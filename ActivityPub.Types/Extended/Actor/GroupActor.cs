// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Actor;

/// <summary>
/// Represents a formal or informal collective of Actors. 
/// </summary>
public class GroupActor : ASActor
{
    private GroupActorEntity Entity { get; }

    public GroupActor() => Entity = new GroupActorEntity(TypeMap);
    public GroupActor(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<GroupActorEntity>();
}

/// <inheritdoc cref="GroupActor"/>
[ASTypeKey(GroupType)]
[ImpliesOtherEntity(typeof(ASActorEntity))]
public sealed class GroupActorEntity : ASBase<GroupActor>
{
    public const string GroupType = "Group";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASObjectEntity.ObjectType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public GroupActorEntity(TypeMap typeMap) : base(typeMap, GroupType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public GroupActorEntity() : base(GroupType, ReplacedTypes) {}
}