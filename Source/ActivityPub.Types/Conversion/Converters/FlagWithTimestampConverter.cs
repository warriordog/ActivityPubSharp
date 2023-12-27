// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Conversion.Converters;

/// <summary>
///     Custom JSON converter for <see cref="FlagWithTimestamp"/>.
/// </summary>
public class FlagWithTimestampConverter : JsonConverter<FlagWithTimestamp>
{
    /// <inheritdoc />
    public override FlagWithTimestamp? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.True => new FlagWithTimestamp { Value = true },
            JsonTokenType.False => new FlagWithTimestamp { Value = false },
            JsonTokenType.String => new FlagWithTimestamp { Timestamp = reader.GetDateTime() },
            _ => throw new JsonException($"Can't convert {reader.TokenType} as {nameof(FlagWithTimestamp)}")
        };

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, FlagWithTimestamp value, JsonSerializerOptions options)
    {
        if (value.Timestamp != null)
            writer.WriteStringValue(value.Timestamp.Value);
        else
            writer.WriteBooleanValue(value.Value);
    }
}