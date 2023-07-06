// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Internal;

namespace ActivityPub.Types.Json;

/// <summary>
/// Converts ASType and descendents into the correct subclass based on runtime information provided in <see cref="ASType.Types"/>.
/// </summary>
/// <remarks>
/// Important: this should NOT be used in attribute form!
/// Please add it to the serializer options to ensure that it applies to all subclasses.
/// </remarks>
public class ASTypeConverter : JsonConverterFactory
{
    private readonly ASTypeRegistry _sharedTypeRegistry = ASTypeRegistry.Create();

    // We can convert any subclass of ASType
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsAssignableTo(typeof(ASType));

    // Pivot the target type directly to the generic argument, then call the default constructor
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        // Create JsonOptions
        var jsonOptions = new JsonOptions(options, _sharedTypeRegistry);

        // Create an instance of the generic converter
        var constructedType = typeof(ASTypeConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(
            constructedType,
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: new object[] { jsonOptions },
            culture: null
        )!;
    }
}

/// <summary>
/// Please use <see cref="ASTypeConverter"/> (non-generic factory) instead.
/// </summary>
/// <typeparam name="T">specific type to produce</typeparam>
internal class ASTypeConverter<T> : JsonConverter<T>
    where T : ASType, IJsonConvertible<T>
{
    private readonly JsonOptions _jsonOptions;
    public ASTypeConverter(JsonOptions jsonOptions) => _jsonOptions = jsonOptions;

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Initially parse into an abstract form.
        // We will introspect to find the correct subtype, then convert from abstract into concrete POCO.
        var objectElement = JsonElement.ParseValue(ref reader);

        // Narrow the type to construct
        var actualType = GetTargetType(objectElement, typeToConvert);

        // If different, then re-enter the serializer to correct the type of T
        if (actualType != typeToConvert)
        {
            if (!actualType.IsAssignableTo(typeToConvert))
                throw new JsonException($"Cannot deserialize JSON message of type {actualType} into object of type {typeToConvert}");

            return (T?)objectElement.Deserialize(actualType, _jsonOptions.SerializerOptions);
        }

        // If we get here, then T is the correct type.
        // That means we can use it to access the per-type logic/
        return T.Deserialize(objectElement, _jsonOptions);
    }

    private Type GetTargetType(JsonElement objectElement, Type typeToConverter)
    {
        // If the object contains a type, then use that to reify as much as possible
        if (objectElement.TryGetASType(out var asTypeName))
            return _jsonOptions.TypeRegistry.ReifyType<T>(asTypeName);

        // If not, then use the declared type AS LONG AS its not the base ASType
        if (typeToConverter != typeof(ASType))
            return typeToConverter;

        // Finally, fall back to ASObject
        return typeof(ASObject);
    }

    // For writer, we just call per-type logic.
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var node = T.Serialize(value, _jsonOptions);
        node.WriteTo(writer, options);
    }
}