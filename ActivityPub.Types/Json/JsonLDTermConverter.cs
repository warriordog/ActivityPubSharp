using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Json;

/// <summary>
/// JSON converter for <see cref="JsonLDTerm"/>.
/// </summary>
/// <seealso cref="JsonLDContextConverter"/>
/// <seealso cref="JsonLDContextPropertyConverter"/>
public class JsonLDTermConverter : JsonConverter<JsonLDTerm>
{
    public override JsonLDTerm? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;

            case JsonTokenType.String:
            {
                var id = reader.GetString()!;
                return new JsonLDTerm { Id = id };
            }

            case JsonTokenType.StartObject:
                return JsonSerializer.Deserialize<JsonLDExpandedTerm>(ref reader, options);

            default:
                throw new JsonException($"Cannot deserialize {reader.TokenType} as JsonLDTerm");
        }
    }

    public override void Write(Utf8JsonWriter writer, JsonLDTerm value, JsonSerializerOptions options)
    {
        // We can only get the non-expanded (string) form here
        writer.WriteStringValue(value.Id);
    }
}