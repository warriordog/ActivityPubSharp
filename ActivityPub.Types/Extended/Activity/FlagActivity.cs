// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is "flagging" the object.
/// Flagging is defined in the sense common to many social platforms as reporting content as being inappropriate for any number of reasons. 
/// </summary>
[ASTypeKey(FlagType)]
public class FlagActivity : ASTransitiveActivity
{
    public const string FlagType = "Flag";

    [JsonConstructor]
    public FlagActivity() : this(FlagType) {}

    protected FlagActivity(string type) : base(type) {}
}