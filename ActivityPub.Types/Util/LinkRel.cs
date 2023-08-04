// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Conversion.Converters;

namespace ActivityPub.Types.Util;

/// <summary>
///     A link relation compatible with <see cref="ASLink.Rel" />.
///     Any string not containing the "space" U+0020, "tab" (U+0009), "LF" (U+000A), "FF" (U+000C), "CR" (U+000D) or "," (U+002C) characters can be used as a valid link relation.
/// </summary>
[JsonConverter(typeof(LinkRelConverter))]
public record LinkRel
{
    // Characters that cannot appear within a link relation.
    // Values are: "space" ( ), "tab" (\t), "line feed" (\n), "carriage return" (\r), and "comma" (,)
    private static readonly char[] IllegalChars = { '\u0020', '\u0009', '\u000A', '\u000C', '\u000D', '\u002C' };

    public LinkRel(string value)
    {
        var badCharIdx = value.IndexOfAny(IllegalChars);
        if (badCharIdx > -1)
            throw new ArgumentException($"LinkRel contains illegal character at position {badCharIdx}", nameof(value));

        Value = value;
    }

    public string Value { get; }

    public static implicit operator string(LinkRel linkRel) => linkRel.Value;
    public static implicit operator LinkRel(string str) => new(str);
}