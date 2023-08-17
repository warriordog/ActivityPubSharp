// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents an organization.
/// </summary>
public class OrganizationActor : ASActor
{
    public OrganizationActor() => Entity = new OrganizationActorEntity { TypeMap = TypeMap };
    public OrganizationActor(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<OrganizationActorEntity>();
    private OrganizationActorEntity Entity { get; }
}

/// <inheritdoc cref="OrganizationActor" />
[APType(OrganizationType)]
[ImpliesOtherEntity(typeof(ASActorEntity))]
public sealed class OrganizationActorEntity : ASEntity<OrganizationActor>
{
    public const string OrganizationType = "Organization";
    public override string ASTypeName => OrganizationType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASObjectEntity.ObjectType
    };
}