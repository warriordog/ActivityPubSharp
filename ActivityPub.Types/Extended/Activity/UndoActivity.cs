// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is undoing the object.
/// In most cases, the object will be an Activity describing some previously performed action (for instance, a person may have previously "liked" an article but, for whatever reason, might choose to undo that like at some later point in time).
/// The target and origin typically have no defined meaning. 
/// </summary>
[APType(UndoType)]
public class UndoActivity : ASTransitiveActivity
{
    public const string UndoType = "Undo";

    [JsonConstructor]
    public UndoActivity() : this(UndoType) {}

    protected UndoActivity(string type) : base(type) {}
}
