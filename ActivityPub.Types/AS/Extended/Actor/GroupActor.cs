// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents a formal or informal collective of Actors.
/// </summary>
public class GroupActor : ASActor
{
    public GroupActor() => Entity = new GroupActorEntity { TypeMap = TypeMap };
    public GroupActor(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<GroupActorEntity>();
    private GroupActorEntity Entity { get; }
}

/// <inheritdoc cref="GroupActor" />
[ASTypeKey(GroupType)]
[ImpliesOtherEntity(typeof(ASActorEntity))]
public sealed class GroupActorEntity : ASBase<GroupActor>
{
    public const string GroupType = "Group";
    public override string ASTypeName => GroupType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASObjectEntity.ObjectType
    };
}