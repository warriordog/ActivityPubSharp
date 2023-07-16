// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

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
/// Please use <see cref="ASTypeConverter"/> (non-generic factory) instead.
/// </summary>
/// <typeparam name="T">specific type to produce</typeparam>
internal class ASTypeConverter<T> : JsonConverter<T>
    where T : ASType
{
    private readonly IASTypeInfoCache _asTypeInfoCache;
    private readonly IJsonTypeInfoCache _jsonTypeInfoCache;
    public ASTypeConverter(IASTypeInfoCache asTypeInfoCache, IJsonTypeInfoCache jsonTypeInfoCache)
    {
        _asTypeInfoCache = asTypeInfoCache;
        _jsonTypeInfoCache = jsonTypeInfoCache;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // 1. Parse into abstract form (JsonElement)
        var inputElement = JsonElement.ParseValue(ref reader);

        // 2. Find serialization info for the appropriate .NET type
        var typeInfo = GetTargetType(inputElement, typeToConvert);

        // 3. Convert it
        return ReadObject(inputElement, typeInfo, options);
    }

    private JsonTypeInfo GetTargetType(JsonElement objectElement, Type typeToConverter)
    {
        // If the object contains a type, then use that to reify as much as possible
        if (objectElement.TryGetASType(out var asTypeName))
            return _asTypeInfoCache.GetJsonTypeInfo<T>(asTypeName);

        // If not, then use the declared type AS LONG AS its not the base ASType
        if (typeToConverter != typeof(ASType))
            return _jsonTypeInfoCache.GetForType<T>();

        // Finally, fall back to ASObject
        return _jsonTypeInfoCache.GetForType<ASObject>();
    }

    private static T ReadObject(JsonElement inputElement, JsonTypeInfo typeInfo, JsonSerializerOptions options)
    {
        // Attempt to call the custom deserializer
        if (typeInfo.TryDeserialize(inputElement, options, out var obj))
            return (T)obj;

        // If it fails or isn't present, then fall back to default (manual) logic
        return (T)ReadObjectDirectly(inputElement, typeInfo, options);
    }

    private static object ReadObjectDirectly(JsonElement inputElement, JsonTypeInfo typeInfo, JsonSerializerOptions options)
    {
        // Create instance
        var obj = Activator.CreateInstance(typeInfo.Type);
        if (obj == null)
            throw new JsonException($"Failed to construct an instance of {typeInfo.Type}");

        // Populate values
        foreach (var prop in typeInfo.Setters)
        {
            // Attempt to load the value from json
            if (inputElement.TryGetProperty(prop.Name, out var valueElement))
            {
                var value = valueElement.Deserialize(prop.Property.PropertyType, options);
                prop.Property.SetValue(obj, value);
                continue;
            }

            // If missing & required, then throw an exception
            if (prop.IsRequired)
                throw new JsonException($"Cannot parse {typeInfo.Type} - missing required property '{prop.Name}'");
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

        // Create JsonNodeOptions because System.Text.Json has so many different "option" types ...
        // TODO find out if we can cache this somehow
        var nodeOptions = new JsonNodeOptions
        {
            PropertyNameCaseInsensitive = options.PropertyNameCaseInsensitive
        };

        // Call custom serializer.
        // If it fails or is missing, then serialize manually
        if (!typeInfo.TrySerialize(value, options, nodeOptions, out var node))
            node = WriteObjectDirectly(value, typeInfo, nodeOptions);

        // Write it to the output stream
        node.WriteTo(writer, options);
    }

    private static JsonNode WriteObjectDirectly(T value, JsonTypeInfo typeInfo, JsonNodeOptions nodeOptions)
    {
        // Manually create an object and append each property.
        // Its dumb, but this bypasses the design flaw of System.Text.Json.
        var node = new JsonObject(nodeOptions);
        foreach (var prop in typeInfo.Getters)
        {
            node[prop.Name] = JsonValue.Create(prop.Property.GetValue(value), nodeOptions);
        }

        return node;
    }
}