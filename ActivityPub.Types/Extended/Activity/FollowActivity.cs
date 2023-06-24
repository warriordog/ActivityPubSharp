/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is "following" the object.
/// Following is defined in the sense typically used within Social systems in which the actor is interested in any activity performed by or on the object.
/// The target and origin typically have no defined meaning. 
/// </summary>
[ASTypeKey(FollowType)]
public class FollowActivity : ASTransitiveActivity
{
    public const string FollowType = "Follow";

    [JsonConstructor]
    public FollowActivity() : this(FollowType) {}

    protected FollowActivity(string type) : base(type) {}
}