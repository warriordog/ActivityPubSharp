/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is "flagging" the object.
/// Flagging is defined in the sense common to many social platforms as reporting content as being inappropriate for any number of reasons. 
/// </summary>
public class FlagActivity : ASTransitiveActivity
{
    public const string FlagType = "Flag";
    public FlagActivity(string type = FlagType) : base(type) {}
}