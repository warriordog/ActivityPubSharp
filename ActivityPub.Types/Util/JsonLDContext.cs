// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Util;

/// <summary>
/// A JSON-LD context object.
/// Can be embedded within a document or referenced by a link.
/// </summary>
/// <seealso cref="JsonLDTerm"/>
/// <seealso href="https://www.w3.org/TR/json-ld11/#dfn-context"/>
/// <seealso href="https://www.w3.org/TR/json-ld11/#context-definitions"/>
/// <seealso href="https://www.w3.org/TR/json-ld11/#the-context"/>
[JsonConverter(typeof(JsonLDContextConverter))]
public class JsonLDContext : IEquatable<JsonLDContext>
{
    /// <summary>
    /// If true, then this context is located externally and we only have a link.
    /// </summary>
    [MemberNotNullWhen(true, nameof(ExternalLink))]
    [MemberNotNullWhen(false, nameof(Terms))]
    public bool IsExternal => ExternalLink != null;

    /// <summary>
    /// If true, then this context is embedded directly in an object and all terms are available.
    /// </summary>
    [MemberNotNullWhen(false, nameof(ExternalLink))]
    [MemberNotNullWhen(true, nameof(Terms))]
    public bool IsEmbedded => Terms != null;

    /// <summary>
    /// For an externally linked context, this is the actual link.
    /// WARNING: this may be a relative URI!
    /// </summary>
    public string? ExternalLink { get; }

    /// <summary>
    /// For an embedded context, these are the terms defined.
    /// Important: terms may be defined by *other* external contexts.
    /// </summary>
    public IReadOnlyDictionary<string, JsonLDTerm>? Terms { get; }

    /// <summary>
    /// Construct an external context
    /// </summary>
    /// <param name="externalLink">Link to the external context file</param>
    public JsonLDContext(string externalLink) => ExternalLink = externalLink;

    /// <summary>
    /// Construct an embedded context
    /// </summary>
    /// <param name="terms">Terms to include in the context</param>
    public JsonLDContext(IReadOnlyDictionary<string, JsonLDTerm> terms) => Terms = terms;

    /// <summary>
    /// If this is an external context, gets the link value.
    /// </summary>
    public bool TryGetLink([NotNullWhen(true)] out string? link)
    {
        link = ExternalLink;
        return IsExternal;
    }

    /// <summary>
    /// If this is an embedded context, gets the terms.
    /// </summary>
    public bool TryGetTerms([NotNullWhen(true)] out IReadOnlyDictionary<string, JsonLDTerm>? terms)
    {
        terms = Terms;
        return IsEmbedded;
    }

    public bool Equals(JsonLDContext? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ExternalLink == other.ExternalLink && Equals(Terms, other.Terms);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((JsonLDContext)obj);
    }
    public override int GetHashCode() => HashCode.Combine(ExternalLink, Terms);

    public static implicit operator JsonLDContext(string str) => new(str);
}