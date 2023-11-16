// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents a formal or informal collective of Actors.
/// </summary>
public class GroupActor : APActor, IASModel<GroupActor, GroupActorEntity, APActor>
{
    public const string GroupType = "Group";
    static string IASModel<GroupActor>.ASTypeName => GroupType;

    public GroupActor() : this(new TypeMap()) {}

    public GroupActor(TypeMap typeMap) : base(typeMap)
    {
        Entity = new GroupActorEntity();
        TypeMap.Add(Entity);
    }

    public GroupActor(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public GroupActor(TypeMap typeMap, GroupActorEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<GroupActorEntity>();

    static GroupActor IASModel<GroupActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private GroupActorEntity Entity { get; }
}

/// <inheritdoc cref="GroupActor" />
public sealed class GroupActorEntity : ASEntity<GroupActor, GroupActorEntity> {}