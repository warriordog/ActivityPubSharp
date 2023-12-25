// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is undoing the object.
///     In most cases, the object will be an Activity describing some previously performed action (for instance, a person may have previously "liked" an article but, for whatever reason, might choose to undo that like at some later point in time).
///     The target and origin typically have no defined meaning.
/// </summary>
public class UndoActivity : ASActivity, IASModel<UndoActivity, UndoActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Undo" types.
    /// </summary>
    [PublicAPI]
    public const string UndoType = "Undo";
    static string IASModel<UndoActivity>.ASTypeName => UndoType;

    /// <inheritdoc />
    public UndoActivity() => Entity = TypeMap.Extend<UndoActivity, UndoActivityEntity>();

    /// <inheritdoc />
    public UndoActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<UndoActivity, UndoActivityEntity>(isExtending);

    /// <inheritdoc />
    public UndoActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public UndoActivity(TypeMap typeMap, UndoActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<UndoActivity, UndoActivityEntity>();

    static UndoActivity IASModel<UndoActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private UndoActivityEntity Entity { get; }
}

/// <inheritdoc cref="UndoActivity" />
public sealed class UndoActivityEntity : ASEntity<UndoActivity, UndoActivityEntity> {}