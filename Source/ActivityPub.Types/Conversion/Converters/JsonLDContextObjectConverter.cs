// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Conversion.Converters;

/// <summary>
///     Custom converter for <see cref="JsonLDContextObject" />
/// </summary>
/// <seealso cref="JsonLDTermConverter" />
/// <seealso cref="JsonLDContextConverter" />
internal class JsonLDContextObjectConverter : JsonConverter<JsonLDContextObject>
{
    public override JsonLDContextObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;

            case JsonTokenType.String:
            {
                var str = reader.GetString()!;
                return new JsonLDContextObject(str);
            }

            case JsonTokenType.StartObject:
            {
                var terms = JsonSerializer.Deserialize<Dictionary<string, JsonLDTerm>>(ref reader, options)?.AsReadOnly();
                if (terms == null)
                    throw new JsonException("Failed to parse JsonLDContextObject terms");

                return new JsonLDContextObject(terms);
            }

            default:
                throw new JsonException($"Cannot deserialize {reader.TokenType} as JsonLDContextObject");
        }
    }

    public override void Write(Utf8JsonWriter writer, JsonLDContextObject value, JsonSerializerOptions options)
    {
        if (value.IsExternal)
            writer.WriteStringValue(value.ExternalLink);
        else
            JsonSerializer.Serialize(writer, value.Terms, options);
    }
}