// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ActivityPub.Types.AS;
using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Internal.Pivots;
using ActivityPub.Types.Util;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace ActivityPub.Types.Conversion.Converters;

/// <inheritdoc />
public class TypeMapConverter : JsonConverter<TypeMap>
{
    private readonly IASTypeInfoCache _asTypeInfoCache;
    private readonly IConversionOptions _conversionOptions;
    private readonly NamelessEntityPivot _namelessEntityPivot = new();
    private readonly AnonymousEntityPivot _anonymousEntityPivot = new();
    private readonly CustomConvertedEntityPivot _customConvertedEntityPivot = new();

    /// <inheritdoc />
    [UsedImplicitly]
    public TypeMapConverter(IASTypeInfoCache asTypeInfoCache, IOptions<ConversionOptions> conversionOptions)
    {
        _asTypeInfoCache = asTypeInfoCache;
        _conversionOptions = conversionOptions.Value;
    }

    /// <inheritdoc />
    public TypeMapConverter()
    {
        _asTypeInfoCache = ASTypeInfoCache.Instance;
        _conversionOptions = new ConversionOptions();
    }

    /// <inheritdoc />
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
        
        ReadKnownEntities(jsonElement, meta);
        ReadNamelessEntities(jsonElement, meta);
        ReadAnonymousEntities(jsonElement, meta);

        return typeMap;
    }
    
    private void ReadKnownEntities(JsonElement jsonElement, DeserializationMetadata meta)
    {
        // String input is a special case of Link
        if (jsonElement.ValueKind == JsonValueKind.String)
            ReadString(jsonElement, meta.TypeMap);

        // Object input can be anything
        else if (jsonElement.ValueKind == JsonValueKind.Object)
            ReadObject(jsonElement, meta);

        // Other input is an error
        else
            throw new JsonException($"Can't convert TypeMap from {jsonElement.ValueKind}");
    }
    private static void ReadString(JsonElement jsonElement, TypeMap typeMap)
    {
        // Read Link entity
        var link = new ASLinkEntity
        {
            HRef = jsonElement.GetString()!
        };
        typeMap.AddEntity(link);

        // Create and attach empty Type entity
        var type = new ASTypeEntity();
        typeMap.AddEntity(type);
    }

    private void ReadObject(JsonElement jsonElement, DeserializationMetadata meta)
    {
        // Enumerate and expand the full list of needed types
        var asTypes = ReadTypes(jsonElement, meta.JsonSerializerOptions);
        _asTypeInfoCache.MapASTypesToEntities(asTypes, out var mappedTypes, out var unmappedTypes);

        // Convert each AS type that mapped to an object type
        foreach (var entityType in mappedTypes)
            ReadEntity(jsonElement, meta, entityType);

        // Record each AS type that did *not* map to an object type
        foreach (var asType in unmappedTypes)
            meta.TypeMap.AddUnmappedType(asType);
    }

    private void ReadNamelessEntities(JsonElement jsonElement, DeserializationMetadata meta)
    {
        // Nameless entities must be checked for every input JSON.
        // Any that match are converted like normal entities, but skipping the usual AS type lookup.
        foreach (var entityType in _asTypeInfoCache.NamelessEntityTypes)
            if (_namelessEntityPivot.ShouldConvert(entityType, meta))
                ReadEntity(jsonElement, meta, entityType);
    }
    
    private void ReadAnonymousEntities(JsonElement jsonElement, DeserializationMetadata meta)
    {
        // Anonymous entities must be checked for every input JSON.
        // Any that match are converted like normal entities, but skipping the usual AS type / context lookup.
        foreach (var entityType in _asTypeInfoCache.AnonymousEntityTypes)
            if (_anonymousEntityPivot.ShouldConvert(entityType, jsonElement, meta))
                ReadEntity(jsonElement, meta, entityType);
        
        // Registered anonymous entity selectors must also run for every input.
        // They can each identify any number of anonymous entities.
        foreach (var selector in _conversionOptions.AnonymousEntitySelectors)
            foreach (var entityType in selector.SelectAnonymousEntities(jsonElement, meta))
                ReadEntity(jsonElement, meta, entityType);
    }

    private void ReadEntity(JsonElement jsonElement, DeserializationMetadata meta, Type entityType)
    {
        // Skip if the entity is already in the graph
        if (meta.TypeMap.AllEntities.ContainsKey(entityType))
            return;

        var entity =
            // Use custom logic, if applicable
            _customConvertedEntityPivot.ReadEntity(entityType, jsonElement, meta)
        
            // Fall back to native JSON conversion
            ?? (ASEntity?)jsonElement.Deserialize(entityType, meta.JsonSerializerOptions)
        
            // Bail out if both fail
            ?? throw new JsonException($"Failed to deserialize {entityType} - JsonElement.Deserialize returned null");

        // Execute callback, if applicable
        _customConvertedEntityPivot.PostReadEntity(entityType, jsonElement, meta, entity);
        
        // Add it to the graph.
        meta.TypeMap.AddEntity(entity);

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
        types.Add(ASObject.ObjectType);

        return types;
    }

    private static JsonLDContext ReadContext(JsonElement jsonElement, JsonSerializerOptions options)
    {
        // Try to get the context property.
        if (jsonElement.ValueKind != JsonValueKind.Object || !jsonElement.TryGetProperty("@context", out var contextProp))
            // If missing then use default.
            return JsonLDContext.CreateASContext();

        // Convert context
        return contextProp.Deserialize<JsonLDContext>(options)
               ?? throw new JsonException("Can't convert TypeMap - \"@context\" is null");
    }

    /// <inheritdoc />
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
        // Convert to an intermediate object.
        // This will contain the subset of fields that are owned by this entity.
        var element = 
            // Use custom converter, if applicable
            _customConvertedEntityPivot.WriteEntity(entityType, entity, meta)
            
            // Fall back to native conversion
            ?? JsonSerializer.SerializeToElement(entity, entityType, meta.JsonSerializerOptions);
        
        // Sanity check
        if (element.ValueKind != JsonValueKind.Object)
            throw new JsonException($"Failed to write {entityType} to object - serialization produced unsupported JSON type {element.ValueKind}");

        // Copy all properties
        foreach (var property in element.EnumerateObject())
        {
            var valueNode = property.Value.ToNode(meta.JsonNodeOptions);
            outputNode[property.Name] = valueNode;
        }
        
        // Execute callback, if applicable
        _customConvertedEntityPivot.PostWriteEntity(entityType, entity, meta, element, outputNode);
    }

    private void WriteTypeMap(TypeMap typeMap, JsonObject outputNode, SerializationMetadata meta)
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
        // If there are any unmapped properties, then bail
        if (typeMap.UnmappedProperties?.Any() == true)
            return false;

        // If there is any data in any link entities, then bail
        if (typeMap.AllEntities.Values.Any(link => link.RequiresObjectForm))
            return false;

        // If there is no ASLinkEntity, then bail
        if (!typeMap.IsEntity<ASLinkEntity>(out var linkEntity))
            return false;

        // Finally - its safe to write string form
        var href = linkEntity.HRef?.ToString();
        if (href == null)
            writer.WriteNullValue();
        else
            writer.WriteStringValue(href);
        return true;
    }
}