// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has deleted the object.
///     If specified, the origin indicates the context from which the object was deleted.
/// </summary>
public class DeleteActivity : ASActivity, IASModel<DeleteActivity, DeleteActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Delete" types.
    /// </summary>
    public const string DeleteType = "Delete";
    static string IASModel<DeleteActivity>.ASTypeName => DeleteType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public DeleteActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public DeleteActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<DeleteActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public DeleteActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public DeleteActivity(TypeMap typeMap, DeleteActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<DeleteActivityEntity>();

    static DeleteActivity IASModel<DeleteActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private DeleteActivityEntity Entity { get; }
}

/// <inheritdoc cref="DeleteActivity" />
public sealed class DeleteActivityEntity : ASEntity<DeleteActivity, DeleteActivityEntity> {}