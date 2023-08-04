// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is removing the object.
///     If specified, the origin indicates the context from which the object is being removed.
/// </summary>
public class RemoveActivity : ASTargetedActivity
{
    public RemoveActivity() => Entity = new RemoveActivityEntity { TypeMap = TypeMap };
    public RemoveActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<RemoveActivityEntity>();
    private RemoveActivityEntity Entity { get; }
}

/// <inheritdoc cref="RemoveActivity" />
[ASTypeKey(RemoveType)]
[ImpliesOtherEntity(typeof(ASTargetedActivityEntity))]
public sealed class RemoveActivityEntity : ASBase<RemoveActivity>
{
    public const string RemoveType = "Remove";
    public override string ASTypeName => RemoveType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}