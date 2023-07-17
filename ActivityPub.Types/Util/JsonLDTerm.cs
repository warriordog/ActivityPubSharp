// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Util;

/// <summary>
/// Within a context, maps a short word to an IRI. 
/// </summary>
/// <seealso cref="JsonLDContextObject"/>
/// <seealso href="https://www.w3.org/TR/json-ld11/#dfn-term"/>
/// <seealso href="https://www.w3.org/TR/json-ld11/#terms"/>
[JsonConverter(typeof(JsonLDTermConverter))]
public class JsonLDTerm : IEquatable<JsonLDTerm>
{
    /// <summary>
    /// A short-hand string that expands to an IRI, blank node identifier, or keyword.
    /// </summary>
    [JsonPropertyName("@id")]
    public required string Id { get; init; }

    public bool Equals(JsonLDTerm? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((JsonLDTerm)obj);
    }
    public override int GetHashCode() => Id.GetHashCode();
}

/// <summary>
/// Expanded form of a term that carries additional information
/// </summary>
/// <seealso href="https://www.w3.org/TR/json-ld11/#dfn-expanded-term-definition"/>
/// <seealso href="https://www.w3.org/TR/json-ld11/#expanded-term-definition"/>
public class JsonLDExpandedTerm : JsonLDTerm, IEquatable<JsonLDExpandedTerm>
{
    /// <summary>
    /// idk...
    /// </summary>
    [JsonPropertyName("@type")]
    public string? Type { get; init; }

    public bool Equals(JsonLDExpandedTerm? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && Type == other.Type;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((JsonLDExpandedTerm)obj);
    }
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Type);
}