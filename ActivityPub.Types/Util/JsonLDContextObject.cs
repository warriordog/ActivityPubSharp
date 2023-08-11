// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ActivityPub.Types.Conversion.Converters;

namespace ActivityPub.Types.Util;

/// <summary>
///     A JSON-LD context object.
///     Can be embedded within a document or referenced by a link.
/// </summary>
/// <seealso cref="JsonLDTerm" />
/// <seealso href="https://www.w3.org/TR/json-ld11/#dfn-context" />
/// <seealso href="https://www.w3.org/TR/json-ld11/#context-definitions" />
/// <seealso href="https://www.w3.org/TR/json-ld11/#the-context" />
[JsonConverter(typeof(JsonLDContextObjectConverter))]
public record JsonLDContextObject
{
    /// <summary>
    ///     Construct an external context
    /// </summary>
    /// <param name="externalLink">Link to the external context file</param>
    public JsonLDContextObject(string externalLink) => ExternalLink = externalLink;

    /// <summary>
    ///     Construct an embedded context
    /// </summary>
    /// <param name="terms">Terms to include in the context</param>
    public JsonLDContextObject(IReadOnlyDictionary<string, JsonLDTerm> terms) => _terms = new TermMap(terms);

    /// <summary>
    ///     Immutable, shared reference to the ActivityStreams context.
    /// </summary>
    public static JsonLDContextObject ActivityStreams { get; } = new("https://www.w3.org/ns/activitystreams");

    /// <summary>
    ///     If true, then this context is located externally and we only have a link.
    /// </summary>
    [MemberNotNullWhen(true, nameof(ExternalLink))]
    [MemberNotNullWhen(false, nameof(Terms))]
    public bool IsExternal => ExternalLink != null;

    /// <summary>
    ///     If true, then this context is embedded directly in an object and all terms are available.
    /// </summary>
    [MemberNotNullWhen(false, nameof(ExternalLink))]
    [MemberNotNullWhen(true, nameof(Terms))]
    public bool IsEmbedded => Terms != null;

    /// <summary>
    ///     For an externally linked context, this is the actual link.
    ///     WARNING: this may be a relative URI!
    /// </summary>
    public string? ExternalLink { get; }

    /// <summary>
    ///     For an embedded context, these are the terms defined.
    ///     Important: terms may be defined by *other* external contexts.
    /// </summary>
    public IReadOnlyDictionary<string, JsonLDTerm>? Terms => _terms?.Data;

    private readonly TermMap? _terms;

    /// <summary>
    ///     If this is an external context, gets the link value.
    /// </summary>
    public bool TryGetLink([NotNullWhen(true)] out string? link)
    {
        link = ExternalLink;
        return IsExternal;
    }

    /// <summary>
    ///     If this is an embedded context, gets the terms.
    /// </summary>
    public bool TryGetTerms([NotNullWhen(true)] out IReadOnlyDictionary<string, JsonLDTerm>? terms)
    {
        terms = Terms;
        return IsEmbedded;
    }

    public static implicit operator JsonLDContextObject(string str) => new(str);

    private sealed class TermMap : IEquatable<TermMap>
    {
        private readonly Dictionary<string, JsonLDTerm> _data;
        public IReadOnlyDictionary<string, JsonLDTerm> Data => _data;

        public TermMap(IReadOnlyDictionary<string, JsonLDTerm> data) => _data = new Dictionary<string, JsonLDTerm>(data);


        public bool Equals(TermMap? other)
        {
            if (null == other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (_data.Count != other._data.Count)
                return false;

            foreach (var (key, value) in _data)
            {
                if (!other._data.TryGetValue(key, out var otherValue))
                    return false;

                if (!value.Equals(otherValue))
                    return false;
            }

            return true;
        }

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is TermMap other && Equals(other);

        public override int GetHashCode()
            => _data.Aggregate(1, (h, e) => HashCode.Combine(h, e.Key.GetHashCode(), e.Value.GetHashCode()));
    }
}