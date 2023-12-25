// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Conversion.Converters;

namespace ActivityPub.Types.Util;

/// <summary>
///     A JSON-LD context.
///     Contains a set of context objects.
/// </summary>
/// <seealso cref="JsonLDContextObject" />
/// <seealso href="https://www.w3.org/TR/json-ld11/#dfn-context" />
/// <seealso href="https://www.w3.org/TR/json-ld11/#context-definitions" />
/// <seealso href="https://www.w3.org/TR/json-ld11/#the-context" />
public interface IJsonLDContext : IReadOnlySet<JsonLDContextObject>
{
    /// <inheritdoc cref="JsonLDContextObject.ActivityStreams"/>
    public static IJsonLDContext ActivityStreams { get; } = JsonLDContext.CreateASContext();
}

/// <summary>
///     Mutable implementation of <see cref="IJsonLDContext"/>.
/// </summary>
[JsonConverter(typeof(JsonLDContextConverter))]
public class JsonLDContext : HashSet<JsonLDContextObject>, IJsonLDContext
{
    /// <summary>
    ///     Constructs an empty <see cref="JsonLDContext"/>.
    /// </summary>
    public JsonLDContext() {}

    /// <summary>
    ///     Constructs a <see cref="JsonLDContext"/> from a collection of context objects
    /// </summary>
    /// <param name="objects">Objects to form the whole context</param>
    public JsonLDContext(IEnumerable<JsonLDContextObject> objects) : base(objects) {}

    /// <summary>
    ///     Constructs a new context, pre-initialized with the ActivityStreams context.
    /// </summary>
    /// <seealso cref="JsonLDContextObject.ActivityStreams"/>
    public static JsonLDContext CreateASContext() => new()
    {
        JsonLDContextObject.ActivityStreams
    };
    
    /// <summary>
    ///     Creates a shallow copy of this Json-LD context.
    /// </summary>
    public JsonLDContext Clone() => new(this);
}