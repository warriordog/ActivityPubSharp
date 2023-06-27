// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has updated the object.
/// Note, however, that this vocabulary does not define a mechanism for describing the actual set of modifications made to object.
/// The target and origin typically have no defined meaning. 
/// </summary>
[ASTypeKey(UndoType)]
public class UpdateActivity : ASTransitiveActivity
{
    public const string UndoType = "Update";

    [JsonConstructor]
    public UpdateActivity() : this(UndoType) {}

    protected UpdateActivity(string type) : base(type) {}
}