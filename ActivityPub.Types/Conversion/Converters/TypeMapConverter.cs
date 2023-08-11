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
using InternalUtils;

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


        // String input is a special case of Link
        if (jsonElement.ValueKind == JsonValueKind.String)
            ReadString(jsonElement, typeMap);

        // Object input can be anything
        else if (jsonElement.ValueKind == JsonValueKind.Object)
            ReadObject(jsonElement, meta);

        // Other input is an error
        else
            throw new JsonException($"Can't convert TypeMap from {jsonElement.ValueKind}");

        return typeMap;
    }

    private static void ReadString(JsonElement jsonElement, TypeMap typeMap)
    {
        // Read Link entity
        var link = new ASLinkEntity
        {
            HRef = jsonElement.GetString()!
        };
        typeMap.Add(link);

        // Create and attach empty Type entity
        var type = new ASTypeEntity();
        typeMap.Add(type);
    }

    private void ReadObject(JsonElement jsonElement, DeserializationMetadata meta)
    {
        // Enumerate and expand the full list of needed types
        var asTypes = ReadTypes(jsonElement, meta.JsonSerializerOptions);
        _asTypeInfoCache.MapASTypes(asTypes, out var mappedTypes, out var unmappedTypes);

        // Convert each AS type that mapped to an object type
        foreach (var entityType in mappedTypes)
            ReadEntity(jsonElement, meta, entityType);

        // Record each AS type that did *not* map to an object type
        foreach (var asType in unmappedTypes)
            meta.TypeMap.AddUnmappedType(asType);
    }

    private void ReadEntity(JsonElement jsonElement, DeserializationMetadata meta, Type entityType)
    {
        // We need to *also* convert any more-specific types, recursively
        var adapters = GetAdaptersFor(entityType);
        if (adapters.PickSubTypeForDeserializationAdapter?.TryNarrowTypeByJson(jsonElement, meta, out var narrowType) == true)
            ReadEntity(jsonElement, meta, narrowType);

        // Use default conversion
        var entity = (ASEntity?)jsonElement.Deserialize(entityType, meta.JsonSerializerOptions)
                     ?? throw new JsonException($"Failed to deserialize {entityType} - JsonElement.Deserialize returned null");

        // Add it to the graph.
        // TryAdd is needed
        meta.TypeMap.TryAdd(entity);

        // Remove the entity-level set.
        // We need it for conversion, but only until TypeMap is updated
        entity.UnmappedProperties = null;
    }

    private static IEnumerable<string> ReadTypes(JsonElement jsonElement, JsonSerializerOptions options)
    {
        HashSet<string> types;

        // Try to read types
        if (jsonElement.TryGetProperty("type", out var typeProp))
            types = typeProp.Deserialize<HashSet<string>>(options)
                    ?? throw new JsonException("Can't convert TypeMap - \"type\" is null");
        else
            types = new HashSet<string>();

        // Make sure we always have object
        types.Add(ASObjectEntity.ObjectType);

        return types;
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
            TypeMap = typeMap,
            JsonSerializerOptions = options,
            JsonNodeOptions = options.ToNodeOptions()
        };

        // Links require special handling.
        // If the *only* property is href, then they compact to a string
        if (TryWriteAsLink(writer, typeMap))
            return;

        // Create node to hold the output
        var outputNode = new JsonObject(meta.JsonNodeOptions);

        // Write the TypeMap's own properties into the node
        WriteTypeMap(typeMap, outputNode, meta);

        // Write all entities into the node
        foreach (var (entityType, entity) in typeMap.AllEntities)
        {
            // Serialize with default logic
            WriteEntity(entity, entityType, outputNode, meta);
        }

        // Write the node
        outputNode.WriteTo(writer, options);
    }

    private void WriteEntity(ASEntity entity, Type entityType, JsonObject outputNode, SerializationMetadata meta)
    {
        // Attempt TrySerialize
        var adapters = GetAdaptersFor(entityType);
        if (adapters.TrySerializeAdapter?.TrySerialize(entity, meta, outputNode) == true)
            return;

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

    private static void WriteTypeMap(TypeMap typeMap, JsonObject outputNode, SerializationMetadata meta)
    {
        // "type" - AS / AP types. Can be string or array.
        outputNode["type"] = JsonSerializer.SerializeToNode(typeMap.ASTypes, meta.JsonSerializerOptions);

        // "@context" - JSON-LD context. Can be string, array, or object.
        outputNode["@context"] = JsonSerializer.SerializeToNode(typeMap.LDContext, meta.JsonSerializerOptions);

        // Unmapped (overflow) JSON properties
        if (typeMap.UnmappedProperties != null)
            foreach (var (key, value) in typeMap.UnmappedProperties)
                outputNode[key] = value.ToNode(meta.JsonNodeOptions);
    }

    private static bool TryWriteAsLink(Utf8JsonWriter writer, TypeMap typeMap)
    {
        var linkEntities = typeMap.LinkEntities.ToList();

        // If there are any unmapped properties, then bail
        if (typeMap.UnmappedProperties?.Any() == true)
            return false;

        // If there are any non-link entities, then bail
        if (typeMap.AllEntities.Count > linkEntities.Count)
            return false;

        // If there is any data in any link entities, then bail
        if (linkEntities.Any(link => link.RequiresObjectForm))
            return false;

        // If there is no ASLinkEntity, then bail
        if (!typeMap.IsEntity<ASLinkEntity>(out var linkEntity))
            return false;

        // Finally - its safe to write string form
        writer.WriteStringValue(linkEntity.HRef);
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
        TrySerializeAdapter = TrySerializeAdapter.CreateFor(type),
        PickSubTypeForDeserializationAdapter = PickSubTypeForDeserializationAdapter.CreateFor(type)
    };

    private class ASBaseAdapters
    {
        public TrySerializeAdapter? TrySerializeAdapter { get; init; }
        public PickSubTypeForDeserializationAdapter? PickSubTypeForDeserializationAdapter { get; init; }
    }

    private abstract class TrySerializeAdapter
    {
        public abstract bool TrySerialize(ASEntity obj, SerializationMetadata meta, JsonObject node);

        public static TrySerializeAdapter? CreateFor(Type type)
        {
            if (!type.IsAssignableToGenericType(typeof(ICustomJsonSerialized<>)))
                return null;

            var genericType = typeof(TrySerializeAdapter<>).MakeGenericType(type);
            return (TrySerializeAdapter)Activator.CreateInstance(genericType)!;
        }
    }

    private class TrySerializeAdapter<T> : TrySerializeAdapter
        where T : ASEntity, ICustomJsonSerialized<T>
    {
        public override bool TrySerialize(ASEntity obj, SerializationMetadata meta, JsonObject node) => T.TrySerialize((T)obj, meta, node);
    }

    private abstract class PickSubTypeForDeserializationAdapter
    {
        public abstract bool TryNarrowTypeByJson(JsonElement element, DeserializationMetadata meta, [NotNullWhen(true)] out Type? type);

        public static PickSubTypeForDeserializationAdapter? CreateFor(Type type)
        {
            if (!type.IsAssignableTo(typeof(ISubTypeDeserialized)))
                return null;

            var genericType = typeof(PickSubTypeForDeserializationAdapter<>).MakeGenericType(type);
            return (PickSubTypeForDeserializationAdapter)Activator.CreateInstance(genericType)!;
        }
    }

    private class PickSubTypeForDeserializationAdapter<T> : PickSubTypeForDeserializationAdapter
        where T : ISubTypeDeserialized
    {
        public override bool TryNarrowTypeByJson(JsonElement element, DeserializationMetadata meta, [NotNullWhen(true)] out Type? type)
            => T.TryNarrowTypeByJson(element, meta, out type);
    }
}