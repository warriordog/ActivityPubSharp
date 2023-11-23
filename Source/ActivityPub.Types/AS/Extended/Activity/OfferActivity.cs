// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

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
    public const string OfferType = "Offer";
    static string IASModel<OfferActivity>.ASTypeName => OfferType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public OfferActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public OfferActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<OfferActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public OfferActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public OfferActivity(TypeMap typeMap, OfferActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<OfferActivityEntity>();

    static OfferActivity IASModel<OfferActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private OfferActivityEntity Entity { get; }
}

/// <inheritdoc cref="OfferActivity" />
public sealed class OfferActivityEntity : ASEntity<OfferActivity, OfferActivityEntity> {}