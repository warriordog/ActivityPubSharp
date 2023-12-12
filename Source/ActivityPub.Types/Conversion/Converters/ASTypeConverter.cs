// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.AS;

namespace ActivityPub.Types.Conversion.Converters;

/// <inheritdoc />
public class ASTypeConverter : JsonConverterFactory
{
    private readonly Type _asTypeType = typeof(ASType);


    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsAssignableTo(_asTypeType);

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        // Create an instance of the generic converter
        var constructedType = typeof(ASTypeConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(constructedType)!;
    }
}

internal class ASTypeConverter<T> : JsonConverter<T>
    where T : ASType, IASModel<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Its possible for typeToConvert to be a subtype of T.
        // In that case, we MUST re-enter with the correct type!
        if (typeToConvert != typeof(T))
            return (T?)JsonSerializer.Deserialize(ref reader, typeToConvert, options);

        // Conversion is mostly just constructing a TypeMap with all the data
        var typeMap = JsonSerializer.Deserialize<TypeMap>(ref reader, options)
                      ?? throw new JsonException($"Can't convert {typeof(T)}: conversion to TypeMap returned null");

        // Then we just construct the target type. 
        return typeMap.AsModel<T>();
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        // Simple pass-through of TypeMap
        => JsonSerializer.Serialize(writer, value.TypeMap, options);
}