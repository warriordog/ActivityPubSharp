// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Conversion.Converters;

/// <summary>
///     Converts language-tagged strings
/// </summary>
internal class NaturalLanguageStringConverter : JsonConverter<NaturalLanguageString>
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
                return new NaturalLanguageString()
                {
                    DefaultValue = str
                };
            }

            case JsonTokenType.StartObject:
            {
                var languageTags = JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, options)!;
                return NaturalLanguageString.FromLanguageMap(languageTags);
            }

            default:
                throw new JsonException($"Cannot convert {reader.TokenType} into NaturalLanguageString");
        }
    }

    public override void Write(Utf8JsonWriter writer, NaturalLanguageString value, JsonSerializerOptions options)
    {
        // If it has any language mappings, then we have to use object form
        if (value.LanguageMap.Any())
            JsonSerializer.Serialize(writer, value.LanguageMap, options);
        
        // Otherwise, we can use the default value
        else if (value.DefaultValue != null)
            writer.WriteStringValue(value.DefaultValue);
        
        // Fall back to null
        else
            writer.WriteNullValue();
    }
}