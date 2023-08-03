// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is undoing the object.
/// In most cases, the object will be an Activity describing some previously performed action (for instance, a person may have previously "liked" an article but, for whatever reason, might choose to undo that like at some later point in time).
/// The target and origin typically have no defined meaning. 
/// </summary>
public class UndoActivity : ASTransitiveActivity
{
    private UndoActivityEntity Entity { get; }

    public UndoActivity() => Entity = new UndoActivityEntity(TypeMap);
    public UndoActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<UndoActivityEntity>();
}

/// <inheritdoc cref="UndoActivity"/>
[ASTypeKey(UndoType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class UndoActivityEntity : ASBase
{
    public const string UndoType = "Undo";

    /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public UndoActivityEntity(TypeMap typeMap) : base(UndoType, typeMap) {}

    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public UndoActivityEntity() : base(UndoType) {}
}