// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using ActivityPub.Types.Json;

namespace ActivityPub.Common.Util;

/// <summary>
/// Injected into all JSON-LD classes to provide common settings 
/// </summary>
public class ActivityPubOptions
{
    /// <summary>
    /// Default options for the JSON-LD serializer
    /// </summary>
    public JsonSerializerOptions SerializerOptions { get; set; } = JsonLdSerializerOptions.Default;

    /// <summary>
    /// Content types to recognize as ActivityPub
    /// </summary>
    public HashSet<string> ContentTypes { get; set; } = new()
    {
        "application/ld+json",
        "application/activity+json",
        "application/json"
    };
}