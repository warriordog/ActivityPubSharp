// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Json;

/// <summary>
/// Converts types that can be either <see cref="ASLink"/> or some other type.
/// </summary>
public class LinkableConverter : JsonConverterFactory
{
    // We only convert Linkable<T>
    public override bool CanConvert(Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Linkable<>);

    // Pivot the type into correct instance
    public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
    {
        var valueType = type.GetGenericArguments()[0];
        var converterType = typeof(LinkableConverter<>).MakeGenericType(valueType);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

internal class LinkableConverter<T> : JsonConverter<Linkable<T>>
    where T : ASObject
{
    public override Linkable<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;

        // Parse into abstract form
        var jsonElement = JsonElement.ParseValue(ref reader);

        // If its a string OR an object of type ASLink, then its a link
        if (jsonElement.ValueKind == JsonValueKind.String || IsASLink(jsonElement))
        {
            // Delegate link construction back to the parser
            var link = jsonElement.Deserialize<ASLink>(options);
            if (link == null)
                throw new JsonException($"Failed to parse {typeToConvert} - Could not construct link object");
            return new Linkable<T>(link);
        }

        // Otherwise, its the payload data
        var obj = jsonElement.Deserialize<T>(options);
        if (obj == null)
            throw new JsonException($"Failed to parse {typeToConvert} - Could not construct value object");
        return new Linkable<T>(obj);
    }

    public override void Write(Utf8JsonWriter writer, Linkable<T> linkable, JsonSerializerOptions options)
    {
        // It should be OK to use basic JsonSerializer here, because the important stuff is all in the options instance.

        if (linkable.TryGetLink(out var link))
        {
            JsonSerializer.Serialize(writer, link, options);
        }
        else if (linkable.TryGetValue(out var value))
        {
            JsonSerializer.Serialize(writer, value, options);
        }
        else
        {
            throw new ArgumentException($"{typeof(Linkable<T>)} is invalid - it has neither a link nor a value");
        }
    }

    private static bool IsASLink(JsonElement element) => element.TryGetASType(out var type) && type == ASLink.LinkType;
}