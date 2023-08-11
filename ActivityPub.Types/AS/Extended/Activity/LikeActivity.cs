// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor likes, recommends or endorses the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class LikeActivity : ASTransitiveActivity
{
    public LikeActivity() => Entity = new LikeActivityEntity { TypeMap = TypeMap };
    public LikeActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<LikeActivityEntity>();
    private LikeActivityEntity Entity { get; }
}

/// <inheritdoc cref="LikeActivity" />
[ASTypeKey(LikeType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class LikeActivityEntity : ASEntity<LikeActivity>
{
    public const string LikeType = "Like";
    public override string ASTypeName => LikeType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}