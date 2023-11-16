// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has deleted the object.
///     If specified, the origin indicates the context from which the object was deleted.
/// </summary>
public class DeleteActivity : ASTransitiveActivity, IASModel<DeleteActivity, DeleteActivityEntity, ASTransitiveActivity>
{
    public const string DeleteType = "Delete";
    static string IASModel<DeleteActivity>.ASTypeName => DeleteType;

    public DeleteActivity() : this(new TypeMap()) {}

    public DeleteActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<DeleteActivityEntity>();

    public DeleteActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public DeleteActivity(TypeMap typeMap, DeleteActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<DeleteActivityEntity>();

    static DeleteActivity IASModel<DeleteActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private DeleteActivityEntity Entity { get; }
}

/// <inheritdoc cref="DeleteActivity" />
public sealed class DeleteActivityEntity : ASEntity<DeleteActivity, DeleteActivityEntity> {}