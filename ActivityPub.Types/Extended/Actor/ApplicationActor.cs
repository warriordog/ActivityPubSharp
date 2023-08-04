// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Actor;

/// <summary>
/// Describes a software application. 
/// </summary>
public class ApplicationActor : ASActor
{
    private ApplicationActorEntity Entity { get; }

    public ApplicationActor() => Entity = new ApplicationActorEntity { TypeMap = TypeMap };
    public ApplicationActor(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ApplicationActorEntity>();
}

/// <inheritdoc cref="ApplicationActor"/>
[ASTypeKey(ApplicationType)]
[ImpliesOtherEntity(typeof(ASActorEntity))]
public sealed class ApplicationActorEntity : ASBase<ApplicationActor>
{
    public const string ApplicationType = "Application";
    public override string ASTypeName => ApplicationType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>()
    {
        ASObjectEntity.ObjectType
    };
}