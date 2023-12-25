// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents a formal or informal collective of Actors.
/// </summary>
public class GroupActor : APActor, IASModel<GroupActor, GroupActorEntity, APActor>
{
    /// <summary>
    ///     ActivityStreams type name for "Group" types.
    /// </summary>
    [PublicAPI]
    public const string GroupType = "Group";
    static string IASModel<GroupActor>.ASTypeName => GroupType;

    /// <inheritdoc />
    public GroupActor() => Entity = TypeMap.Extend<GroupActor, GroupActorEntity>();

    /// <inheritdoc />
    public GroupActor(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<GroupActor, GroupActorEntity>(isExtending);

    /// <inheritdoc />
    public GroupActor(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public GroupActor(TypeMap typeMap, GroupActorEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<GroupActor, GroupActorEntity>();

    static GroupActor IASModel<GroupActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private GroupActorEntity Entity { get; }
}

/// <inheritdoc cref="GroupActor" />
public sealed class GroupActorEntity : ASEntity<GroupActor, GroupActorEntity> {}