// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is calling the target's attention the object.
/// The origin typically has no defined meaning. 
/// </summary>
[APType(AnnounceType)]
public class AnnounceActivity : ASTransitiveActivity
{
    public const string AnnounceType = "Announce";

    [JsonConstructor]
    public AnnounceActivity() : this(AnnounceType) {}

    protected AnnounceActivity(string type) : base(type) {}
}
