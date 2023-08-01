// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Actor;

/// <summary>
/// Represents a service of any kind.
/// </summary>
public class ServiceActor : ASActor
{
    private ServiceActorEntity Entity { get; }
    
    public ServiceActor() => Entity = new ServiceActorEntity(TypeMap);
    public ServiceActor(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ServiceActorEntity>();
}


/// <inheritdoc cref="ServiceActor"/>
[ASTypeKey(ServiceType)]
public sealed class ServiceActorEntity : ASBase
{
    public const string ServiceType = "Service";

    public ServiceActorEntity(TypeMap typeMap) : base(ServiceType, typeMap) {}
}