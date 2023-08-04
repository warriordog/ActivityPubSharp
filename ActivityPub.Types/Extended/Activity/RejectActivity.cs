// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is rejecting the object.
/// The target and origin typically have no defined meaning. 
/// </summary>
[APType(RejectType)]
public class RejectActivity : ASTransitiveActivity
{
    public const string RejectType = "Reject";

    [JsonConstructor]
    public RejectActivity() : this(RejectType) {}

    protected RejectActivity(string type) : base(type) {}
}
