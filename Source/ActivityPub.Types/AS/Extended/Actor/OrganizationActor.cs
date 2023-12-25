// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents an organization.
/// </summary>
public class OrganizationActor : APActor, IASModel<OrganizationActor, OrganizationActorEntity, APActor>
{
    /// <summary>
    ///     ActivityStreams type name for "Organization" types.
    /// </summary>
    [PublicAPI]
    public const string OrganizationType = "Organization";
    static string IASModel<OrganizationActor>.ASTypeName => OrganizationType;

    /// <inheritdoc />
    public OrganizationActor() => Entity = TypeMap.Extend<OrganizationActor, OrganizationActorEntity>();

    /// <inheritdoc />
    public OrganizationActor(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<OrganizationActor, OrganizationActorEntity>(isExtending);

    /// <inheritdoc />
    public OrganizationActor(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public OrganizationActor(TypeMap typeMap, OrganizationActorEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<OrganizationActor, OrganizationActorEntity>();

    static OrganizationActor IASModel<OrganizationActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private OrganizationActorEntity Entity { get; }
}

/// <inheritdoc cref="OrganizationActor" />
public sealed class OrganizationActorEntity : ASEntity<OrganizationActor, OrganizationActorEntity> {}