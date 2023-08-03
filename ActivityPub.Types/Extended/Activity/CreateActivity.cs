// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has created the object.
/// </summary>
[APType(CreateType)]
public class CreateActivity : ASTransitiveActivity
{
    public const string CreateType = "Create";

    [JsonConstructor]
    public CreateActivity() : this(CreateType) {}

    protected CreateActivity(string type) : base(type) {}
}
