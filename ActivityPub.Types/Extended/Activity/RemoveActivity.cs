// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is removing the object.
/// If specified, the origin indicates the context from which the object is being removed. 
/// </summary>
[APType(RemoveType)]
public class RemoveActivity : ASTargetedActivity
{
    public const string RemoveType = "Remove";

    [JsonConstructor]
    public RemoveActivity() : this(RemoveType) {}

    protected RemoveActivity(string type) : base(type) {}
}
