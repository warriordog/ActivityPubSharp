/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is calling the target's attention the object.
/// The origin typically has no defined meaning. 
/// </summary>
public class AnnounceActivity : ASTransitiveActivity
{
    public const string AnnounceType = "Announce";
    public AnnounceActivity(string type = AnnounceType) : base(type) {}
}