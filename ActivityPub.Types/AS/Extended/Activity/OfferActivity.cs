// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is offering the object.
///     If specified, the target indicates the entity to which the object is being offered.
/// </summary>
public class OfferActivity : ASTransitiveActivity
{
    public OfferActivity() => Entity = new OfferActivityEntity { TypeMap = TypeMap };
    public OfferActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<OfferActivityEntity>();
    private OfferActivityEntity Entity { get; }
}

/// <inheritdoc cref="OfferActivity" />
[ASTypeKey(OfferType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class OfferActivityEntity : ASBase<OfferActivity>
{
    public const string OfferType = "Offer";
    public override string ASTypeName => OfferType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}