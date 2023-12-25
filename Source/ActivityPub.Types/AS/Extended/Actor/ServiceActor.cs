// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents a service of any kind.
/// </summary>
public class ServiceActor : APActor, IASModel<ServiceActor, ServiceActorEntity, APActor>
{
    /// <summary>
    ///     ActivityStreams type name for "Service" types.
    /// </summary>
    [PublicAPI]
    public const string ServiceType = "Service";
    static string IASModel<ServiceActor>.ASTypeName => ServiceType;

    /// <inheritdoc />
    public ServiceActor() => Entity = TypeMap.Extend<ServiceActor, ServiceActorEntity>();

    /// <inheritdoc />
    public ServiceActor(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<ServiceActor, ServiceActorEntity>(isExtending);

    /// <inheritdoc />
    public ServiceActor(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public ServiceActor(TypeMap typeMap, ServiceActorEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ServiceActor, ServiceActorEntity>();

    static ServiceActor IASModel<ServiceActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private ServiceActorEntity Entity { get; }
}

/// <inheritdoc cref="ServiceActor" />
public sealed class ServiceActorEntity : ASEntity<ServiceActor, ServiceActorEntity> {}