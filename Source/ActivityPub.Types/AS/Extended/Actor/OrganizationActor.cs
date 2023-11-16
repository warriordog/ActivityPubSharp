// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents an organization.
/// </summary>
public class OrganizationActor : APActor, IASModel<OrganizationActor, OrganizationActorEntity, APActor>
{
    /// <summary>
    ///     ActivityStreams type name for "Organization" types.
    /// </summary>
    public const string OrganizationType = "Organization";
    static string IASModel<OrganizationActor>.ASTypeName => OrganizationType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public OrganizationActor() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public OrganizationActor(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<OrganizationActorEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public OrganizationActor(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public OrganizationActor(TypeMap typeMap, OrganizationActorEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<OrganizationActorEntity>();

    static OrganizationActor IASModel<OrganizationActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private OrganizationActorEntity Entity { get; }
}

/// <inheritdoc cref="OrganizationActor" />
public sealed class OrganizationActorEntity : ASEntity<OrganizationActor, OrganizationActorEntity> {}