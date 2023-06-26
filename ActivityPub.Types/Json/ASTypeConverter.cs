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
        // Create a copy of options that does not include ASTypeConverter.
        // Without this, we would enter an infinite loop because the converter would call itself.
        var defaultConverterOptions = new JsonSerializerOptions(options).RemoveConvertersOfType<ASTypeConverter>();

        // Create an instance of the generic converter
        var constructedType = typeof(ASTypeConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(
            constructedType,
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: new object[] { _sharedTypeRegistry, defaultConverterOptions },
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
    private readonly ASTypeRegistry _typeRegistry;
    private readonly JsonSerializerOptions _defaultOptions;

    public ASTypeConverter(ASTypeRegistry typeRegistry, JsonSerializerOptions defaultOptions)
    {
        _typeRegistry = typeRegistry;
        _defaultOptions = defaultOptions;
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // We will attempt to narrow these down.
        // On failure, the defaults will be used.
        var actualOptions = _defaultOptions;
        var actualType = typeToConvert;

        // If the JSON object maps to a more specific type, then we need to identify it and then re-enter the parser.
        // This will be false on the second entry which is triggered by the re-entry below.
        if (reader.TryGetASObjectType(out var asTypeName))
        {
            // Find and check the type.
            // Its possible (even likely) for this to be the exact same type, in which case we should skip re-entry for performance.
            var asType = _typeRegistry.GetTypeForName(asTypeName);
            if (asType != typeToConvert)
            {
                if (!asType.IsAssignableTo(typeToConvert))
                    throw new JsonException($"Cannot deserialize JSON message of type {asType} into object of type {typeToConvert}");

                // Change to the new type, preserving the passed-in options
                actualType = asType;
                actualOptions = options;
            }
        }

        // This will either re-enter the serializer with a new type, or will fall through to the default serializer for the original type.
        return (T?)JsonSerializer.Deserialize(ref reader, actualType, actualOptions);
    }

    // For writer, we just re-enter the serializer without ASTypeConverter.
    // This does two things:
    // 1. Selects a more-narrow serializer, if available
    // 2. Allows the default logic to serialize the object.
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, _defaultOptions);
}