// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is "following" the object.
///     Following is defined in the sense typically used within Social systems in which the actor is interested in any activity performed by or on the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class FollowActivity : ASTransitiveActivity
{
    public FollowActivity() => Entity = new FollowActivityEntity { TypeMap = TypeMap };
    public FollowActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<FollowActivityEntity>();
    private FollowActivityEntity Entity { get; }
}

/// <inheritdoc cref="FollowActivity" />
[ASTypeKey(FollowType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class FollowActivityEntity : ASBase<FollowActivity>
{
    public const string FollowType = "Follow";
    public override string ASTypeName => FollowType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}