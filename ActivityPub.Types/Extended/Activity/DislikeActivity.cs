// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor dislikes the object. 
/// </summary>
[APType(DislikeType)]
public class DislikeActivity : ASTransitiveActivity
{
    public const string DislikeType = "Dislike";

    [JsonConstructor]
    public DislikeActivity() : this(DislikeType) {}

    protected DislikeActivity(string type) : base(type) {}
}
