/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor likes, recommends or endorses the object.
/// The target and origin typically have no defined meaning.
/// </summary>
[ASTypeKey(LikeType)]
public class LikeActivity : ASTransitiveActivity
{
    public const string LikeType = "Like";

    [JsonConstructor]
    public LikeActivity() : this(LikeType) {}

    protected LikeActivity(string type) : base(type) {}
}