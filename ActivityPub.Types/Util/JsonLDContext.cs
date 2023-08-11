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
public interface IJsonLDContext : IReadOnlySet<JsonLDContextObject> {}

/// <summary>
///     Mutable implementation of IJsonLDContext.
/// </summary>
[JsonConverter(typeof(JsonLDContextConverter))]
public class JsonLDContext : HashSet<JsonLDContextObject>, IJsonLDContext
{
    /// <summary>
    ///     Constructs an empty JsonLDContext.
    /// </summary>
    public JsonLDContext() {}

    /// <summary>
    ///     Constructs a JsonLDContext from a collection of context objects
    /// </summary>
    /// <param name="objects">Objects to form the whole context</param>
    public JsonLDContext(IEnumerable<JsonLDContextObject> objects) : base(objects) {}

    /// <summary>
    ///     Constructs a new context, pre-initialized with the ActivityStreams context.
    /// </summary>
    public static JsonLDContext ActivityStreams => new()
    {
        JsonLDContextObject.ActivityStreams
    };
}