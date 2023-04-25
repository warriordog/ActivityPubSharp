/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor likes, recommends or endorses the object.
/// The target and origin typically have no defined meaning.
/// </summary>
public class LikeActivity : ASTransitiveActivity
{
    public const string LikeType = "Like";
    public LikeActivity(string type = LikeType) : base(type) {}
}