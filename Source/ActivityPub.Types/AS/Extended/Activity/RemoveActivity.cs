// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is removing the object.
///     If specified, the origin indicates the context from which the object is being removed.
/// </summary>
public class RemoveActivity : ASTargetedActivity, IASModel<RemoveActivity, RemoveActivityEntity, ASTargetedActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Remove" types.
    /// </summary>
    public const string RemoveType = "Remove";
    static string IASModel<RemoveActivity>.ASTypeName => RemoveType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public RemoveActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public RemoveActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<RemoveActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public RemoveActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public RemoveActivity(TypeMap typeMap, RemoveActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<RemoveActivityEntity>();

    static RemoveActivity IASModel<RemoveActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private RemoveActivityEntity Entity { get; }
}

/// <inheritdoc cref="RemoveActivity" />
public sealed class RemoveActivityEntity : ASEntity<RemoveActivity, RemoveActivityEntity> {}