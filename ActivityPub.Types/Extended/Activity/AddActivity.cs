// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
///     Indicates that the actor has added the object to the target.
///     If the target property is not explicitly specified, the target would need to be determined implicitly by context.
///     The origin can be used to identify the context from which the object originated.
/// </summary>
public class AddActivity : ASTargetedActivity
{
    public AddActivity() => Entity = new AddActivityEntity { TypeMap = TypeMap };
    public AddActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<AddActivityEntity>();
    private AddActivityEntity Entity { get; }
}

/// <inheritdoc cref="AddActivity" />
[ASTypeKey(AddType)]
[ImpliesOtherEntity(typeof(ASTargetedActivityEntity))]
public sealed class AddActivityEntity : ASBase<AddActivity>
{
    public const string AddType = "Add";
    public override string ASTypeName => AddType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}