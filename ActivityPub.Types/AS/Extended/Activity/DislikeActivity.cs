// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor dislikes the object.
/// </summary>
public class DislikeActivity : ASTransitiveActivity
{
    public DislikeActivity() => Entity = new DislikeActivityEntity { TypeMap = TypeMap };
    public DislikeActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<DislikeActivityEntity>();
    private DislikeActivityEntity Entity { get; }
}

/// <inheritdoc cref="DislikeActivity" />
[APConvertible(DislikeType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class DislikeActivityEntity : ASEntity<DislikeActivity>
{
    public const string DislikeType = "Dislike";
    public override string ASTypeName => DislikeType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}