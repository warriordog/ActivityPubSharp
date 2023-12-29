// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Common.Util;

/// <summary>
///     Injected into all JSON-LD classes to provide common settings
/// </summary>
public class ActivityPubOptions
{
    /// <summary>
    ///     HTTP responses will only be recognized as ActivityPub if they match one of these content types.
    ///     This maps to the Content-Type header.
    /// </summary>
    /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Type" />
    public HashSet<string> ResponseContentTypes { get; set; } =
    [
        "application/activity+json",
        "application/ld+json",
        "application/json"
    ];

    /// <summary>
    ///     Content types to request from remote servers, in priority order.
    ///     This maps to the Accept header.
    /// </summary>
    /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept" />
    public List<string> RequestContentTypes { get; set; } =
    [
        "application/activity+json",
        "application/ld+json",
        "application/json"
    ];
}