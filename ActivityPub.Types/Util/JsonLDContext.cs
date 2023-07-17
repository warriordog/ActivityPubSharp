// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Util;

// <summary>
/// A JSON-LD context.
/// Contains a set of context objects.
/// </summary>
/// <seealso cref="JsonLDContextObject"/>
/// <seealso href="https://www.w3.org/TR/json-ld11/#dfn-context"/>
/// <seealso href="https://www.w3.org/TR/json-ld11/#context-definitions"/>
/// <seealso href="https://www.w3.org/TR/json-ld11/#the-context"/>
[JsonConverter(typeof(JsonLDContextConverter))]
public class JsonLDContext
{
    public HashSet<JsonLDContextObject> ContextObjects { get; }

    public JsonLDContext(HashSet<JsonLDContextObject> contextObjects) => ContextObjects = contextObjects;
}