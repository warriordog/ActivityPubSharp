// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

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
public sealed class OrganizationActorEntity : ASBase
{
    public const string OrganizationType = "Organization";

    public OrganizationActorEntity(TypeMap typeMap) : base(OrganizationType, typeMap) {}
}