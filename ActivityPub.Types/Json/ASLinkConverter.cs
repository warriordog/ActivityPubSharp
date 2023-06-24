/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */


using System.Text.Json;
using System.Text.Json.Serialization;

namespace ActivityPub.Types.Json;

public class ASLinkConverter : JsonConverter<ASLink>
{
    public override ASLink? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;

        if (reader.TokenType == JsonTokenType.String)
        {
            var str = reader.GetString()!;
            return new ASLink { HRef = str };
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            return JsonSerializer.Deserialize<ASLink>(ref reader, options);
        }

        throw new JsonException($"Cannot convert {reader.TokenType} into ASLink");
    }

    public override void Write(Utf8JsonWriter writer, ASLink link, JsonSerializerOptions options)
    {
        if (HasOnlyHRef(link))
        {
            writer.WriteStringValue(link.HRef);
        }
        else
        {
            JsonSerializer.Serialize(writer, link, options);
        }
    }


    /// <summary>
    /// True if a link contains a value for <see cref="ASLink.HRef"/> only and can therefore be reduced.
    /// </summary>
    /// <remarks>
    /// TODO: This is really a hack and should be replaced.
    /// Its fragile and must be updated whenever <see cref="ASLink"/> or <see cref="ASType"/> is updated.
    /// </remarks> 
    private static bool HasOnlyHRef(ASLink link) =>
        link.HRefLang == null && link.Width == null && link.Height == null && link.Rel.Count == 0 && link.Id == null &&
        link.AttributedTo.Count == 0 && link.Preview == null && link.Name == null && link.MediaType == null;
}