// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is traveling to target from origin.
/// Travel is an IntransitiveObject whose actor specifies the direct object.
/// If the target or origin are not specified, either can be determined by context.
/// </summary>
[APType(TravelType)]
public class TravelActivity : ASIntransitiveActivity
{
    public const string TravelType = "Travel";

    [JsonConstructor]
    public TravelActivity() : this(TravelType) {}

    protected TravelActivity(string type) : base(type) {}
}
