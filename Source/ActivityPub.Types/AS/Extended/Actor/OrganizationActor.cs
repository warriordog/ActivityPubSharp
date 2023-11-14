// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents an organization.
/// </summary>
public class OrganizationActor : APActor, IASModel<OrganizationActor, OrganizationActorEntity, APActor>
{
    public const string OrganizationType = "Organization";
    static string IASModel<OrganizationActor>.ASTypeName => OrganizationType;

    public OrganizationActor() : this(new TypeMap()) {}

    public OrganizationActor(TypeMap typeMap) : base(typeMap)
    {
        Entity = new OrganizationActorEntity();
        TypeMap.Add(Entity);
    }

    [SetsRequiredMembers]
    public OrganizationActor(TypeMap typeMap, OrganizationActorEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<OrganizationActorEntity>();

    static OrganizationActor IASModel<OrganizationActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private OrganizationActorEntity Entity { get; }
}

/// <inheritdoc cref="OrganizationActor" />
public sealed class OrganizationActorEntity : ASEntity<OrganizationActor, OrganizationActorEntity> {}