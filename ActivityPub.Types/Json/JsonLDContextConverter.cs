// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Json;

/// <summary>
/// Custom converter for <see cref="JsonLDContext"/>
/// </summary>
/// <seealso cref="JsonLDTermConverter"/>
/// <seealso cref="JsonLDContextPropertyConverter"/>
public class JsonLDContextConverter : JsonConverter<JsonLDContext>
{
    public override JsonLDContext? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;

            case JsonTokenType.String:
            {
                var str = reader.GetString()!;
                return new JsonLDContext(str);
            }

            case JsonTokenType.StartObject:
            {
                var terms = JsonSerializer.Deserialize<Dictionary<string, JsonLDTerm>>(ref reader, options)?.AsReadOnly();
                if (terms == null)
                    throw new JsonException("Failed to parse JsonLDContext terms");

                return new JsonLDContext(terms);
            }

            default:
                throw new JsonException($"Cannot deserialize {reader.TokenType} as JsonLDContext");
        }
    }

    public override void Write(Utf8JsonWriter writer, JsonLDContext value, JsonSerializerOptions options)
    {
        if (value.IsExternal)
        {
            writer.WriteStringValue(value.ExternalLink);
        }
        else
        {
            JsonSerializer.Serialize(writer, value.Terms, options);
        }
    }
}