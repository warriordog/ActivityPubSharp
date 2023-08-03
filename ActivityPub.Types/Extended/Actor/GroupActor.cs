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
public sealed class GroupActorEntity : ASBase
{
    public const string GroupType = "Group";

    /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public GroupActorEntity(TypeMap typeMap) : base(GroupType, typeMap) {}

    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public GroupActorEntity() : base(GroupType) {}
}