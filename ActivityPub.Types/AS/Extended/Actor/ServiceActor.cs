// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents a service of any kind.
/// </summary>
public class ServiceActor : ASActor
{
    public ServiceActor() => Entity = new ServiceActorEntity { TypeMap = TypeMap };
    public ServiceActor(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ServiceActorEntity>();
    private ServiceActorEntity Entity { get; }
}

/// <inheritdoc cref="ServiceActor" />
[ASTypeKey(ServiceType)]
[ImpliesOtherEntity(typeof(ASActorEntity))]
public sealed class ServiceActorEntity : ASBase<ServiceActor>
{
    public const string ServiceType = "Service";
    public override string ASTypeName => ServiceType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASObjectEntity.ObjectType
    };
}