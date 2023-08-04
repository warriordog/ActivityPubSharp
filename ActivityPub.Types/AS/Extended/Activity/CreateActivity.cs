// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has created the object.
/// </summary>
public class CreateActivity : ASTransitiveActivity
{
    public CreateActivity() => Entity = new CreateActivityEntity { TypeMap = TypeMap };
    public CreateActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<CreateActivityEntity>();
    private CreateActivityEntity Entity { get; }
}

/// <inheritdoc cref="CreateActivity" />
[ASTypeKey(CreateType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class CreateActivityEntity : ASEntity<CreateActivity>
{
    public const string CreateType = "Create";
    public override string ASTypeName => CreateType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}