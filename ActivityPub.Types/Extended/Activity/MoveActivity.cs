// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has moved object from origin to target.
/// If the origin or target are not specified, either can be determined by context. 
/// </summary>
[APType(MoveType)]
public class MoveActivity : ASTransitiveActivity
{
    public const string MoveType = "Move";

    [JsonConstructor]
    public MoveActivity() : this(MoveType) {}

    protected MoveActivity(string type) : base(type) {}
}
