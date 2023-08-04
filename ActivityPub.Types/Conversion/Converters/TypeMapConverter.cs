// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ActivityPub.Types.AS;
using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Conversion.Converters;

public class TypeMapConverter : JsonConverter<TypeMap>
{
    private readonly IASTypeInfoCache _asTypeInfoCache;
    private readonly Dictionary<Type, ASBaseAdapters> _entityAdapters = new();

    public TypeMapConverter(IASTypeInfoCache asTypeInfoCache) => _asTypeInfoCache = asTypeInfoCache;

    public override TypeMap Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Read input into temporary object
        var jsonElement = JsonElement.ParseValue(ref reader);

        // Read JSON-LD context from the input JSON
        var context = ReadContext(jsonElement, options);

        // Construct empty TypeMap.
        // We will build this out progressively.
        var typeMap = new TypeMap(context);

        // Construct context for this conversion cycle
        var meta = new DeserializationMetadata
        {
            JsonSerializerOptions = options,
            LDContext = context,
            TypeMap = typeMap
        };

        // Convert each type found in JSON
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

    private ASEntity ReadEntity(JsonElement jsonElement, DeserializationMetadata meta, Type entityType)
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
        entity = (ASEntity?)jsonElement.Deserialize(entityType, meta.JsonSerializerOptions)
                 ?? throw new JsonException($"Failed to deserialize {entityType} - JsonElement.Deserialize returned null");

        return entity;
    }

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
                        yield return type;
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
        // Construct meta
        var meta = new SerializationMetadata
        {
            JsonSerializerOptions = options,
            JsonNodeOptions = options.ToNodeOptions()
        };

        // Attempt TrySerializeIntoValue
        if (TryWriteAsValue(writer, typeMap, meta))
            return;

        // Create node to hold the output
        var outputNode = new JsonObject(meta.JsonNodeOptions);

        // Write the TypeMap's own properties into the node
        WriteTypeMap(typeMap, outputNode, options);

        // Write all entities into the node
        foreach (var (entityType, entity) in typeMap.AllEntities)
        {
            // Attempt TrySerialize
            var adapters = GetAdaptersFor(entityType);
            if (adapters.TrySerializeAdapter?.TrySerialize(entity, meta, outputNode) == true)
                continue;

            // Serialize with default logic
            WriteEntity(entity, entityType, outputNode, meta);
        }

        // Write the node
        outputNode.WriteTo(writer, options);
    }

    private static void WriteEntity(ASEntity entity, Type entityType, JsonObject outputNode, SerializationMetadata meta)
    {
        // Convert to an intermediate object
        var element = JsonSerializer.SerializeToElement(entity, entityType, meta.JsonSerializerOptions);
        if (element.ValueKind != JsonValueKind.Object)
            throw new JsonException($"Failed to write {entityType} to object - serialization produced unsupported JSON type {element.ValueKind}");

        // Copy all properties
        foreach (var property in element.EnumerateObject())
        {
            var valueNode = property.Value.ToNode(meta.JsonNodeOptions);
            outputNode[property.Name] = valueNode;
        }
    }

    private static void WriteTypeMap(TypeMap typeMap, JsonObject outputNode, JsonSerializerOptions options)
    {
        // "type" - AS / AP types. Can be string or array.
        outputNode["type"] = JsonSerializer.SerializeToNode(typeMap.ASTypes, options);

        // "@context" - JSON-LD context. Can be string, array, or object.
        outputNode["@context"] = JsonSerializer.SerializeToNode(typeMap.LDContext, options);
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
        public abstract bool TryDeserialize(JsonElement element, DeserializationMetadata meta, [NotNullWhen(true)] out ASEntity? obj);

        public static TryDeserializeAdapter CreateFor(Type type)
        {
            var genericType = typeof(TryDeserializeAdapter<>).MakeGenericType(type);
            return (TryDeserializeAdapter)Activator.CreateInstance(genericType)!;
        }
    }

    private class TryDeserializeAdapter<T> : TryDeserializeAdapter
        where T : ASEntity, ICustomJsonDeserialized<T>
    {
        public override bool TryDeserialize(JsonElement element, DeserializationMetadata meta, [NotNullWhen(true)] out ASEntity? obj)
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
        public abstract bool TrySerialize(ASEntity obj, SerializationMetadata meta, JsonObject node);

        public static TrySerializeAdapter CreateFor(Type type)
        {
            var genericType = typeof(TrySerializeAdapter<>).MakeGenericType(type);
            return (TrySerializeAdapter)Activator.CreateInstance(genericType)!;
        }
    }

    private class TrySerializeAdapter<T> : TrySerializeAdapter
        where T : ASEntity, ICustomJsonSerialized<T>
    {
        public override bool TrySerialize(ASEntity obj, SerializationMetadata meta, JsonObject node) => T.TrySerialize((T)obj, meta, node);
    }

    private abstract class TrySerializeIntoValueAdapter
    {
        public abstract bool TrySerializeIntoValue(ASEntity obj, SerializationMetadata meta, [NotNullWhen(true)] out JsonValue? node);

        public static TrySerializeIntoValueAdapter CreateFor(Type type)
        {
            var genericType = typeof(TrySerializeIntoValueAdapter<>).MakeGenericType(type);
            return (TrySerializeIntoValueAdapter)Activator.CreateInstance(genericType)!;
        }
    }

    private class TrySerializeIntoValueAdapter<T> : TrySerializeIntoValueAdapter
        where T : ASEntity, IJsonValueSerialized<T>
    {
        public override bool TrySerializeIntoValue(ASEntity obj, SerializationMetadata meta, [NotNullWhen(true)] out JsonValue? node) => T.TrySerializeIntoValue((T)obj, meta, out node);
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
        public override Type PickSubTypeForDeserialization(JsonElement element, DeserializationMetadata meta) => T.PickSubTypeForDeserialization(element, meta);
    }
}