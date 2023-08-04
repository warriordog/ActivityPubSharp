// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Conversion.Converters;

/// <summary>
///     Converts language-tagged strings
/// </summary>
public class NaturalLanguageStringConverter : JsonConverter<NaturalLanguageString>
{
    public override NaturalLanguageString? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;

            case JsonTokenType.String:
            {
                var str = reader.GetString()!;
                return new NaturalLanguageString(str);
            }

            case JsonTokenType.StartObject:
            {
                var langStrings = JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, options)!;
                return new NaturalLanguageString(langStrings);
            }

            default:
                throw new JsonException($"Cannot convert {reader.TokenType} into NaturalLanguageString");
        }
    }

    public override void Write(Utf8JsonWriter writer, NaturalLanguageString value, JsonSerializerOptions options)
    {
        if (value.SingleString != null)
            writer.WriteStringValue(value.SingleString);
        else
            JsonSerializer.Serialize(writer, value.LanguageMap, options);
    }
}