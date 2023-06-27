// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// An IntransitiveActivity that indicates that the actor has arrived at the location.
/// The origin can be used to identify the context from which the actor originated.
/// The target typically has no defined meaning. 
/// </summary>
[ASTypeKey(ArriveType)]
public class ArriveActivity : ASIntransitiveActivity
{
    public const string ArriveType = "Arrive";

    [JsonConstructor]
    public ArriveActivity() : this(ArriveType) {}

    protected ArriveActivity(string type) : base(type) {}
}