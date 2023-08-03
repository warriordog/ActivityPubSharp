// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Conversion.Converters;

/// <summary>
/// Custom converter for the JSON-LD "@context" property.
/// </summary>
/// <remarks>
/// We need THREE FUCKING CONVERTERS for a minimum-viable implementation!
/// </remarks>
/// <seealso cref="JsonLDTermConverter"/>
/// <seealso cref="JsonLDContextObjectConverter"/>
/// <seealso href="https://www.w3.org/TR/json-ld11/#the-context"/>
public class JsonLDContextConverter : JsonConverter<JsonLDContext>
{
    public override JsonLDContext? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;

            case JsonTokenType.String:
            case JsonTokenType.StartObject:
            {
                var context = ReadContext(ref reader, options);
                return new JsonLDContext(new HashSet<JsonLDContextObject>
                {
                    context
                });
            }

            case JsonTokenType.StartArray:
            {
                var set = new HashSet<JsonLDContextObject>();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    var context = ReadContext(ref reader, options);
                    set.Add(context);
                }

                return new JsonLDContext(set);
            }

            default:
                throw new JsonException($"Cannot deserialize {reader.TokenType} as @context field");
        }
    }

    private static JsonLDContextObject ReadContext(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var context = JsonSerializer.Deserialize<JsonLDContextObject>(ref reader, options);
        if (context == null)
            throw new JsonException($"Failed to parse @context field - a context was null or invalid");

        return context;
    }

    public override void Write(Utf8JsonWriter writer, JsonLDContext value, JsonSerializerOptions options)
    {
        if (value.ContextObjects.Count == 1)
        {
            var context = value.ContextObjects.First();
            JsonSerializer.Serialize(writer, context, options);
        }
        else
        {
            writer.WriteStartArray();
            foreach (var context in value.ContextObjects)
            {
                JsonSerializer.Serialize(writer, context, options);
            }

            writer.WriteEndArray();
        }
    }
}