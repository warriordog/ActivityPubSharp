// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Internal.TypeInfo;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Conversion;

public class TypeMapConverter : JsonConverter<TypeMap>
{
    private readonly Dictionary<Type, ASBaseAdapters> _entityAdapters = new();
    private readonly IASTypeInfoCache _asTypeInfoCache;

    public TypeMapConverter(IASTypeInfoCache asTypeInfoCache) => _asTypeInfoCache = asTypeInfoCache;

    public override TypeMap Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // 1. Construct empty TypeMap
        var typeMap = new TypeMap();

        // 2. Read input into temporary object
        var jsonElement = JsonElement.ParseValue(ref reader);

        // 3. Construct context for this conversion cycle
        var context = ReadContext(jsonElement, options);
        var meta = new DeserializationMetadata
        {
            JsonSerializerOptions = options,
            LDContext = context,
            TypeMap = typeMap
        };

        // 4. Convert each type found in JSON
        var asTypes = ReadTypes(jsonElement, options);
        var types = _asTypeInfoCache.MapASTypes(asTypes);
        foreach (var entityType in types)
        {
            // Read entity and attach to map
            var entity = ReadEntity(jsonElement, meta, entityType);
            typeMap.Add(entity);
        }

        return typeMap;
    }

    private ASBase ReadEntity(JsonElement jsonElement, DeserializationMetadata meta, Type entityType)
    {
        // Get adapters for type
        var adapters = GetAdaptersFor(entityType);

        // Attempt TrySerialize
        if (adapters.TryDeserializeAdapter?.TryDeserialize(jsonElement, meta, out var entity) == true)
            return entity;

        // Attempt to narrow the type
        if (adapters.PickSubTypeForDeserializationAdapter != null)
            entityType = adapters.PickSubTypeForDeserializationAdapter.PickSubTypeForDeserialization(jsonElement, meta);

        // Use default conversion
        entity = (ASBase?)jsonElement.Deserialize(entityType, meta.JsonSerializerOptions)
                 ?? throw new JsonException($"Failed to deserialize {entityType} - JsonElement.Deserialize returned null");

        return entity;
    }

    // TODO kind of ugly, should refactor later
    private static IEnumerable<string> ReadTypes(JsonElement jsonElement, JsonSerializerOptions options)
    {
        switch (jsonElement.ValueKind)
        {
            // Object + subtypes
            case JsonValueKind.Object:
            {
                // Try to read types
                if (jsonElement.TryGetProperty("type", out var typeProp))
                {
                    var types = typeProp.Deserialize<HashSet<string>>(options)
                                ?? throw new JsonException("Can't convert TypeMap - \"type\" is null");

                    foreach (var type in types)
                    {
                        yield return type;
                    }
                }

                // Otherwise just use Object
                yield return ASObjectEntity.ObjectType;
                break;
            }

            // Link
            case JsonValueKind.String:
                yield return ASLinkEntity.LinkType;
                break;

            // Oops
            default:
                throw new JsonException($"Can't convert TypeMap - \"type\" is unsupported type {jsonElement.ValueKind}");
        }
    }

    private static JsonLDContext ReadContext(JsonElement jsonElement, JsonSerializerOptions options)
    {
        // Try to get the context property.
        if (jsonElement.ValueKind != JsonValueKind.Object || !jsonElement.TryGetProperty("@context", out var contextProp))
            // If missing then use default.
            return JsonLDContext.ActivityStreams;

        // Convert context
        return contextProp.Deserialize<JsonLDContext>(options)
               ?? throw new JsonException("Can't convert TypeMap - \"@context\" is null");
    }

    public override void Write(Utf8JsonWriter writer, TypeMap typeMap, JsonSerializerOptions options)
    {
        // 1. Construct meta
        var meta = new SerializationMetadata
        {
            JsonSerializerOptions = options,
            JsonNodeOptions = options.ToNodeOptions()
        };

        // 2. Attempt TrySerializeIntoValue
        if (TryWriteAsValue(writer, typeMap, meta))
            return;

        // 3. Create node to hold the output
        var outputNode = new JsonObject(meta.JsonNodeOptions);

        // 4. Write all entities into the node
        foreach (var (entityType, entity) in typeMap.AllEntities)
        {
            // 4.1. Attempt TrySerialize
            var adapters = GetAdaptersFor(entityType);
            if (adapters.TrySerializeAdapter?.TrySerialize(entity, meta, outputNode) == true)
                continue;

            // 4.2. Serialize with default logic
            JsonSerializer.Serialize(writer, entity, entityType, options);
        }

        // 5. Write the node
        outputNode.WriteTo(writer, meta.JsonSerializerOptions);
    }

    private bool TryWriteAsValue(Utf8JsonWriter writer, TypeMap typeMap, SerializationMetadata meta)
    {
        if (typeMap.ValueSerializer == null)
            return false;

        // 1. Get the adapters for the type
        var serializer = typeMap.ValueSerializer;
        var serializerType = serializer.GetType();
        var adapters = GetAdaptersFor(serializerType);

        // 2. Sanity check
        if (adapters.TrySerializeIntoValueAdapter == null)
            throw new JsonException($"Cannot serialize TypeMap: value serializer {serializerType} does not implement IJsonValueSerialized");

        // 3. Convert it!
        if (!adapters.TrySerializeIntoValueAdapter.TrySerializeIntoValue(serializer, meta, out var node))
            return false;

        // 4. Write it
        node.WriteTo(writer, meta.JsonSerializerOptions);
        return true;
    }

    private ASBaseAdapters GetAdaptersFor(Type type)
    {
        if (!_entityAdapters.TryGetValue(type, out var adapters))
        {
            adapters = CreateEntityAdaptersFor(type);
            _entityAdapters[type] = adapters;
        }

        return adapters;
    }

    private static ASBaseAdapters CreateEntityAdaptersFor(Type type) => new()
    {
        TryDeserializeAdapter = TryDeserializeAdapter.CreateFor(type),
        TrySerializeAdapter = TrySerializeAdapter.CreateFor(type),
        TrySerializeIntoValueAdapter = TrySerializeIntoValueAdapter.CreateFor(type),
        PickSubTypeForDeserializationAdapter = PickSubTypeForDeserializationAdapter.CreateFor(type)
    };

    private class ASBaseAdapters
    {
        public TryDeserializeAdapter? TryDeserializeAdapter { get; init; }
        public TrySerializeAdapter? TrySerializeAdapter { get; init; }
        public TrySerializeIntoValueAdapter? TrySerializeIntoValueAdapter { get; init; }
        public PickSubTypeForDeserializationAdapter? PickSubTypeForDeserializationAdapter { get; init; }
    }

    private abstract class TryDeserializeAdapter
    {
        public abstract bool TryDeserialize(JsonElement element, DeserializationMetadata meta, [NotNullWhen(true)] out ASBase? obj);

        public static TryDeserializeAdapter CreateFor(Type type)
        {
            var genericType = typeof(TryDeserializeAdapter<>).MakeGenericType(type);
            return (TryDeserializeAdapter)Activator.CreateInstance(genericType)!;
        }
    }

    private class TryDeserializeAdapter<T> : TryDeserializeAdapter
        where T : ASBase, ICustomJsonDeserialized<T>
    {
        public override bool TryDeserialize(JsonElement element, DeserializationMetadata meta, [NotNullWhen(true)] out ASBase? obj)
        {
            if (T.TryDeserialize(element, meta, out var objT))
            {
                obj = objT;
                return true;
            }

            obj = null;
            return false;
        }
    }

    private abstract class TrySerializeAdapter
    {
        public abstract bool TrySerialize(ASBase obj, SerializationMetadata meta, JsonObject node);

        public static TrySerializeAdapter CreateFor(Type type)
        {
            var genericType = typeof(TrySerializeAdapter<>).MakeGenericType(type);
            return (TrySerializeAdapter)Activator.CreateInstance(genericType)!;
        }
    }

    private class TrySerializeAdapter<T> : TrySerializeAdapter
        where T : ASBase, ICustomJsonSerialized<T>
    {
        public override bool TrySerialize(ASBase obj, SerializationMetadata meta, JsonObject node)
        {
            return T.TrySerialize((T)obj, meta, node);
        }
    }

    private abstract class TrySerializeIntoValueAdapter
    {
        public abstract bool TrySerializeIntoValue(ASBase obj, SerializationMetadata meta, [NotNullWhen(true)] out JsonValue? node);

        public static TrySerializeIntoValueAdapter CreateFor(Type type)
        {
            var genericType = typeof(TrySerializeIntoValueAdapter<>).MakeGenericType(type);
            return (TrySerializeIntoValueAdapter)Activator.CreateInstance(genericType)!;
        }
    }

    private class TrySerializeIntoValueAdapter<T> : TrySerializeIntoValueAdapter
        where T : ASBase, IJsonValueSerialized<T>
    {
        public override bool TrySerializeIntoValue(ASBase obj, SerializationMetadata meta, [NotNullWhen(true)] out JsonValue? node)
        {
            return T.TrySerializeIntoValue((T)obj, meta, out node);
        }
    }

    private abstract class PickSubTypeForDeserializationAdapter
    {
        public abstract Type PickSubTypeForDeserialization(JsonElement element, DeserializationMetadata meta);

        public static PickSubTypeForDeserializationAdapter CreateFor(Type type)
        {
            var genericType = typeof(PickSubTypeForDeserializationAdapter<>).MakeGenericType(type);
            return (PickSubTypeForDeserializationAdapter)Activator.CreateInstance(genericType)!;
        }
    }

    private class PickSubTypeForDeserializationAdapter<T> : PickSubTypeForDeserializationAdapter
        where T : ISubTypeDeserialized
    {
        public override Type PickSubTypeForDeserialization(JsonElement element, DeserializationMetadata meta)
        {
            return T.PickSubTypeForDeserialization(element, meta);
        }
    }
}