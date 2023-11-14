// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is offering the object.
///     If specified, the target indicates the entity to which the object is being offered.
/// </summary>
public class OfferActivity : ASTransitiveActivity, IASModel<OfferActivity, OfferActivityEntity, ASTransitiveActivity>
{
    public const string OfferType = "Offer";
    static string IASModel<OfferActivity>.ASTypeName => OfferType;

    public OfferActivity() : this(new TypeMap()) {}

    public OfferActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new OfferActivityEntity();
        TypeMap.Add(Entity);
    }

    [SetsRequiredMembers]
    public OfferActivity(TypeMap typeMap, OfferActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<OfferActivityEntity>();

    static OfferActivity IASModel<OfferActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private OfferActivityEntity Entity { get; }
}

/// <inheritdoc cref="OfferActivity" />
public sealed class OfferActivityEntity : ASEntity<OfferActivity, OfferActivityEntity> {}