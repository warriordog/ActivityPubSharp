// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
///     Indicates that the actor is calling the target's attention the object.
///     The origin typically has no defined meaning.
/// </summary>
public class AnnounceActivity : ASTransitiveActivity
{
    public AnnounceActivity() => Entity = new AnnounceActivityEntity { TypeMap = TypeMap };
    public AnnounceActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<AnnounceActivityEntity>();
    private AnnounceActivityEntity Entity { get; }
}

/// <inheritdoc cref="AnnounceActivity" />
[ASTypeKey(AnnounceType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class AnnounceActivityEntity : ASBase<AnnounceActivity>
{
    public const string AnnounceType = "Announce";
    public override string ASTypeName => AnnounceType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}