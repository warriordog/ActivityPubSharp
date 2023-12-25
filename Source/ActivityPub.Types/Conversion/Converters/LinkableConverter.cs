// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.AS;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Conversion.Converters;

/// <summary>
///     Converts types that can be either <see cref="ASLink" /> or some other type.
/// </summary>
public class LinkableConverter : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type type) =>
        // We only convert Linkable<T>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Linkable<>);

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
    {
        var valueType = type.GetGenericArguments()[0];
        var converterType = typeof(LinkableConverter<>).MakeGenericType(valueType);
        
        // Pivot the type into correct instance
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

internal class LinkableConverter<T> : JsonConverter<Linkable<T>>
    where T : ASType, IASModel<T>
{
    public override Linkable<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        // Parse into abstract form
        var jsonElement = JsonElement.ParseValue(ref reader);
        var typeMap = jsonElement.Deserialize<TypeMap>(options)
            ?? throw new JsonException($"Failed to parse {typeToConvert} - deserialize returned null");

        // If it's a string, then it's a link
        if (typeMap.IsModel<ASLink>(out var link))
            return new Linkable<T>(link);

        // Anything else is the payload data
        if (typeMap.IsModel<T>(out var obj))
            return new Linkable<T>(obj);

        throw new JsonException($"Failed to parse {typeToConvert} - input cannot be projected to that type");
    }

    public override void Write(Utf8JsonWriter writer, Linkable<T> linkable, JsonSerializerOptions options)
    {
        // It should be OK to use basic JsonSerializer here, because the important stuff is all in the options instance.

        if (linkable.TryGetLink(out var link))
            JsonSerializer.Serialize(writer, link, options);
        else if (linkable.TryGetValue(out var value))
            JsonSerializer.Serialize(writer, value, options);
        else
            throw new ArgumentException($"{typeof(Linkable<T>)} is invalid - it has neither a link nor a value");
    }
}