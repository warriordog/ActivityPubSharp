// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is undoing the object.
///     In most cases, the object will be an Activity describing some previously performed action (for instance, a person may have previously "liked" an article but, for whatever reason, might choose to undo that like at some later point in time).
///     The target and origin typically have no defined meaning.
/// </summary>
public class UndoActivity : ASTransitiveActivity
{
    public UndoActivity() => Entity = new UndoActivityEntity { TypeMap = TypeMap };
    public UndoActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<UndoActivityEntity>();
    private UndoActivityEntity Entity { get; }
}

/// <inheritdoc cref="UndoActivity" />
[APType(UndoType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class UndoActivityEntity : ASEntity<UndoActivity>
{
    public const string UndoType = "Undo";
    public override string ASTypeName => UndoType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}