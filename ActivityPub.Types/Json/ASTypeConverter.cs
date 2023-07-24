// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Internal.TypeInfo;

namespace ActivityPub.Types.Json;

/// <summary>
/// Converts ASType and descendents into the correct subclass based on runtime information provided in <see cref="ASType.Types"/>.
/// </summary>
/// <remarks>
/// Important: this should NOT be used in attribute form!
/// Please add it to the serializer options to ensure that it applies to all subclasses.
/// </remarks>
internal class ASTypeConverter : JsonConverterFactory
{
    private readonly IASTypeInfoCache _asTypeInfoCache;
    private readonly IJsonTypeInfoCache _jsonTypeInfoCache;

    // We can convert any subclass of ASType
    internal ASTypeConverter(IASTypeInfoCache asTypeInfoCache, IJsonTypeInfoCache jsonTypeInfoCache)
    {
        _asTypeInfoCache = asTypeInfoCache;
        _jsonTypeInfoCache = jsonTypeInfoCache;
    }
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsAssignableTo(typeof(ASType));

    // Pivot the target type directly to the generic argument, then call the default constructor
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        // Create an instance of the generic converter
        var constructedType = typeof(ASTypeConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(
            constructedType,
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: new object[] { _asTypeInfoCache, _jsonTypeInfoCache },
            culture: null
        )!;
    }
}

/// <summary>
/// This is public ONLY due to C# limitation.
/// Please use <see cref="ASTypeConverter"/> (non-generic factory) instead.
/// </summary>
/// <typeparam name="T">specific type to produce</typeparam>
public class ASTypeConverter<T> : JsonConverter<T>
    where T : ASType
{
    private readonly IASTypeInfoCache _asTypeInfoCache;
    private readonly IJsonTypeInfoCache _jsonTypeInfoCache;
    public ASTypeConverter(IASTypeInfoCache asTypeInfoCache, IJsonTypeInfoCache jsonTypeInfoCache)
    {
        _asTypeInfoCache = asTypeInfoCache;
        _jsonTypeInfoCache = jsonTypeInfoCache;
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // 1. Parse into abstract form (JsonElement)
        var inputElement = JsonElement.ParseValue(ref reader);

        // 2. Find serialization info for the appropriate .NET type.
        var typeInfo = GetTargetType(inputElement, typeToConvert);

        // 3. Convert it
        return ReadObject(inputElement, typeInfo, options);
    }

    private JsonTypeInfo GetTargetType(JsonElement objectElement, Type typeToConverter)
    {
        // If the object contains a type, then use that to reify as much as possible
        if (objectElement.TryGetASType(out var asTypeName) && _asTypeInfoCache.IsKnownASType(asTypeName))
            return _asTypeInfoCache.GetJsonTypeInfo<T>(asTypeName);

        // If not, then use the declared type AS LONG AS its not the base ASType
        if (typeToConverter != typeof(ASType))
            return _jsonTypeInfoCache.GetForType<T>();

        // Finally, fall back to ASObject
        return _jsonTypeInfoCache.GetForType<ASObject>();
    }

    private static T? ReadObject(JsonElement inputElement, JsonTypeInfo typeInfo, JsonSerializerOptions options)
    {
        // Attempt to call the custom deserializer
        if (typeInfo.TryDeserialize(inputElement, options, out var obj))
            return (T?)obj;

        // If it fails or isn't present, then fall back to default (manual) logic
        return (T)ReadObjectDirectly(inputElement, typeInfo, options);
    }

    private static object ReadObjectDirectly(JsonElement inputElement, JsonTypeInfo typeInfo, JsonSerializerOptions options)
    {
        // Create instance
        var obj = (T?)Activator.CreateInstance(typeInfo.Type);
        if (obj == null)
            throw new JsonException($"Failed to construct an instance of {typeInfo.Type}");
        
        // Populate required props
        foreach (var objProp in typeInfo.RequiredSetters.Values)
        {
            // If missing & required, then throw an exception
            if (!inputElement.TryGetProperty(objProp.Name, out var valueElement))
                throw new JsonException($"Cannot parse {typeInfo.Type} - missing required property '{objProp.Name}'");
            
            // Its there - parse it!
            var value = valueElement.Deserialize(objProp.Property.PropertyType, options);
            objProp.Property.SetValue(obj, value);
        }
        
        // Populate optional props
        foreach (var jsonProp in inputElement.EnumerateObject())
        {
            // Skip if its required - we already got it above
            if (typeInfo.RequiredSetters.ContainsKey(jsonProp.Name))
                continue;
            
            // Attempt to match to a setter
            if (typeInfo.OptionalSetters.TryGetValue(jsonProp.Name, out var objProp))
            {
                var value = jsonProp.Value.Deserialize(objProp.Property.PropertyType, options);
                objProp.Property.SetValue(obj, value);
                continue;
            }
            
            // If not found, then store it as overflow
            obj.UnknownJsonProperties[jsonProp.Name] = jsonProp.Value;
        }

        return obj;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        // If value is a subtype of T, then we need to re-enter with the correct type
        var valueType = value.GetType();
        if (valueType != typeof(T))
        {
            JsonSerializer.Serialize(writer, value, valueType, options);
            return;
        }
        
        // Lookup type info for T
        var typeInfo = _jsonTypeInfoCache.GetForType<T>();
        
        // Check if this is a nested object.
        // Need this info for some checks below
        var isNested = writer.CurrentDepth > 0;

        // Create JsonNodeOptions because System.Text.Json has so many different "option" types ...
        var nodeOptions = new JsonNodeOptions
        {
            PropertyNameCaseInsensitive = options.PropertyNameCaseInsensitive
        };
        
        // Call custom serializer.
        // If it fails or is missing, then serialize manually
        if (!typeInfo.TrySerialize(value, options, nodeOptions, out var node))
            node = WriteObjectDirectly(value, typeInfo, nodeOptions, options, isNested);

        // If this is an object, then write all unmapped JSON properties
        if (node is JsonObject objNode)
            WriteUnmappedJson(value, objNode, nodeOptions);
        
        // Write it to the output stream
        node.WriteTo(writer, options);
    }

    private static JsonNode WriteObjectDirectly(T obj, JsonTypeInfo typeInfo, JsonNodeOptions nodeOptions, JsonSerializerOptions options, bool isNested)
    {
        // Manually create an object and append each property.
        // Its dumb, but this bypasses the design flaw of System.Text.Json.
        var node = new JsonObject(nodeOptions);
        foreach (var prop in typeInfo.Getters)
        {
            // Skip if this is an excluded, non-nestable property
            if (isNested && prop.IgnoreWhenNested)
                continue;
            
            // Delay getting the value until after early tests, just for that tiny bit of performance.
            var value = prop.Property.GetValue(obj);
            
            // Emulate JsonIgnoreCondition to clean up output
            var ignoreCondition = prop.IgnoreCondition ?? options.DefaultIgnoreCondition;
            var stripNull = ignoreCondition is JsonIgnoreCondition.WhenWritingNull or JsonIgnoreCondition.WhenWritingDefault;
            var stripDefault = ignoreCondition == JsonIgnoreCondition.WhenWritingDefault;
            var stripEmpty = ignoreCondition != JsonIgnoreCondition.Never;
            
            // Check if this is a default value that should be skipped.
            if (ShouldSkip(value, prop, stripNull, stripDefault, stripEmpty))
                continue;
            
            node[prop.Name] = JsonValue.Create(value, nodeOptions);
        }

        return node;
    }
    
    private static bool ShouldSkip(object? value, JsonPropertyInfo propInfo, bool stripNull, bool stripDefault, bool stripEmpty)
    {
        // Easy case - just check if its null
        if (stripNull && value == null)
            return true;
        
        // Next case - check if its a default value
        if (stripDefault && Equals(value, propInfo.TypeDefaultValue))
            return true;

        // Final case - check if its an empty collection
        if (stripEmpty && propInfo.IsCollection && value != null && ((ICollection)value).Count == 0)
            return true;

        return false;
    }
    
    private static void WriteUnmappedJson(T obj, JsonObject node, JsonNodeOptions nodeOptions)
    {
        foreach (var (name, value) in obj.UnknownJsonProperties)
        {
            var valueNode = value.ToNode(nodeOptions);
            node[name] = valueNode;
        }
    }
}