// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is undoing the object.
///     In most cases, the object will be an Activity describing some previously performed action (for instance, a person may have previously "liked" an article but, for whatever reason, might choose to undo that like at some later point in time).
///     The target and origin typically have no defined meaning.
/// </summary>
public class UndoActivity : ASTransitiveActivity, IASModel<UndoActivity, UndoActivityEntity, ASTransitiveActivity>
{
    public const string UndoType = "Undo";
    static string IASModel<UndoActivity>.ASTypeName => UndoType;

    public UndoActivity() : this(new TypeMap()) {}

    public UndoActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<UndoActivityEntity>();

    public UndoActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public UndoActivity(TypeMap typeMap, UndoActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<UndoActivityEntity>();

    static UndoActivity IASModel<UndoActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private UndoActivityEntity Entity { get; }
}

/// <inheritdoc cref="UndoActivity" />
public sealed class UndoActivityEntity : ASEntity<UndoActivity, UndoActivityEntity> {}