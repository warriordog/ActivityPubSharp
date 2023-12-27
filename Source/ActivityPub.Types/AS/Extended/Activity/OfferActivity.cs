// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is offering the object.
///     If specified, the target indicates the entity to which the object is being offered.
/// </summary>
public class OfferActivity : ASActivity, IASModel<OfferActivity, OfferActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Offer" types.
    /// </summary>
    [PublicAPI]
    public const string OfferType = "Offer";
    static string IASModel<OfferActivity>.ASTypeName => OfferType;

    /// <inheritdoc />
    public OfferActivity() => Entity = TypeMap.Extend<OfferActivity, OfferActivityEntity>();

    /// <inheritdoc />
    public OfferActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<OfferActivity, OfferActivityEntity>(isExtending);

    /// <inheritdoc />
    public OfferActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public OfferActivity(TypeMap typeMap, OfferActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<OfferActivity, OfferActivityEntity>();

    static OfferActivity IASModel<OfferActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private OfferActivityEntity Entity { get; }
}

/// <inheritdoc cref="OfferActivity" />
public sealed class OfferActivityEntity : ASEntity<OfferActivity, OfferActivityEntity> {}