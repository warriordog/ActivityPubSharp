// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents a service of any kind.
/// </summary>
public class ServiceActor : APActor, IASModel<ServiceActor, ServiceActorEntity, APActor>
{
    /// <summary>
    ///     ActivityStreams type name for "Service" types.
    /// </summary>
    public const string ServiceType = "Service";
    static string IASModel<ServiceActor>.ASTypeName => ServiceType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public ServiceActor() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ServiceActor(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<ServiceActorEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ServiceActor(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public ServiceActor(TypeMap typeMap, ServiceActorEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ServiceActorEntity>();

    static ServiceActor IASModel<ServiceActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private ServiceActorEntity Entity { get; }
}

/// <inheritdoc cref="ServiceActor" />
public sealed class ServiceActorEntity : ASEntity<ServiceActor, ServiceActorEntity> {}