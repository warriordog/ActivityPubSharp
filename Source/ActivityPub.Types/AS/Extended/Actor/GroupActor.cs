// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents a formal or informal collective of Actors.
/// </summary>
public class GroupActor : APActor, IASModel<GroupActor, GroupActorEntity, APActor>
{
    /// <summary>
    ///     ActivityStreams type name for "Group" types.
    /// </summary>
    public const string GroupType = "Group";
    static string IASModel<GroupActor>.ASTypeName => GroupType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public GroupActor() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public GroupActor(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<GroupActorEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public GroupActor(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public GroupActor(TypeMap typeMap, GroupActorEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<GroupActorEntity>();

    static GroupActor IASModel<GroupActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private GroupActorEntity Entity { get; }
}

/// <inheritdoc cref="GroupActor" />
public sealed class GroupActorEntity : ASEntity<GroupActor, GroupActorEntity> {}