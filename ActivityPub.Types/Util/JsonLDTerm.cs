// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Conversion.Converters;

namespace ActivityPub.Types.Util;

/// <summary>
///     Within a context, maps a short word to an IRI.
/// </summary>
/// <seealso cref="JsonLDContextObject" />
/// <seealso href="https://www.w3.org/TR/json-ld11/#dfn-term" />
/// <seealso href="https://www.w3.org/TR/json-ld11/#terms" />
[JsonConverter(typeof(JsonLDTermConverter))]
public record JsonLDTerm
{
    /// <summary>
    ///     A short-hand string that expands to an IRI, blank node identifier, or keyword.
    /// </summary>
    [JsonPropertyName("@id")]
    public required string Id { get; init; }
}

/// <summary>
///     Expanded form of a term that carries additional information
/// </summary>
/// <seealso href="https://www.w3.org/TR/json-ld11/#dfn-expanded-term-definition" />
/// <seealso href="https://www.w3.org/TR/json-ld11/#expanded-term-definition" />
public record JsonLDExpandedTerm : JsonLDTerm
{
    /// <summary>
    ///     idk...
    /// </summary>
    [JsonPropertyName("@type")]
    public string? Type { get; init; }
}