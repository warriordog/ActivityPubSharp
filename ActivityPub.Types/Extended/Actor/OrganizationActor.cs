// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Actor;

/// <summary>
/// Represents an organization. 
/// </summary>
public class OrganizationActor : ASActor
{
    private OrganizationActorEntity Entity { get; }

    public OrganizationActor() => Entity = new OrganizationActorEntity(TypeMap);
    public OrganizationActor(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<OrganizationActorEntity>();
}

/// <inheritdoc cref="OrganizationActor"/>
[ASTypeKey(OrganizationType)]
[ImpliesOtherEntity(typeof(ASActorEntity))]
public sealed class OrganizationActorEntity : ASBase<OrganizationActor>
{
    public const string OrganizationType = "Organization";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASObjectEntity.ObjectType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public OrganizationActorEntity(TypeMap typeMap) : base(typeMap, OrganizationType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public OrganizationActorEntity() : base(OrganizationType, ReplacedTypes) {}
}