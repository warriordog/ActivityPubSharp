// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.AS.Collection;

/// <summary>
///     Used to represent distinct subsets of items from an Ordered Collection.
/// </summary>
/// <remarks>
///     Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the CollectionPage type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-orderedcollectionpage" />
public class ASOrderedCollectionPage : ASOrderedCollection
{
    public ASOrderedCollectionPage() => Entity = new ASOrderedCollectionPageEntity { TypeMap = TypeMap };
    public ASOrderedCollectionPage(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ASOrderedCollectionPageEntity>();
    private ASOrderedCollectionPageEntity Entity { get; }


    /// <summary>
    ///     In a paged Collection, indicates the next page of items.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-next" />
    public Linkable<ASOrderedCollectionPage>? Next
    {
        get => Entity.Next;
        set => Entity.Next = value;
    }

    /// <summary>
    ///     In a paged Collection, indicates the previous page of items.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-prev" />
    public Linkable<ASOrderedCollectionPage>? Prev
    {
        get => Entity.Prev;
        set => Entity.Prev = value;
    }

    /// <summary>
    ///     Identifies the Collection to which a CollectionPage objects items belong.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-partOf" />
    public Linkable<ASOrderedCollection>? PartOf
    {
        get => Entity.PartOf;
        set => Entity.PartOf = value;
    }

    /// <summary>
    ///     A non-negative integer value identifying the relative position within the logical view of a strictly ordered collection.
    ///     This is only valid on OrderedCollectionPage, but it exists on unordered CollectionPage due to technical limitations.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-startIndex" />
    public int? StartIndex
    {
        get => Entity.StartIndex;
        set => Entity.StartIndex = value;
    }

    public static implicit operator ASOrderedCollectionPage(List<ASObject> collection) => new() { Items = new LinkableList<ASObject>(collection) };
}

/// <inheritdoc cref="ASOrderedCollectionPage" />
[ASTypeKey(OrderedCollectionPageType)]
[ImpliesOtherEntity(typeof(ASOrderedCollection))]
public sealed class ASOrderedCollectionPageEntity : ASEntity<ASOrderedCollectionPage>
{
    public const string OrderedCollectionPageType = "OrderedCollectionPage";
    public override string ASTypeName => OrderedCollectionPageType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASOrderedCollectionEntity.OrderedCollectionType
    };

    /// <inheritdoc cref="ASOrderedCollectionPage.Next" />
    [JsonPropertyName("next")]
    public Linkable<ASOrderedCollectionPage>? Next { get; set; }

    /// <inheritdoc cref="ASOrderedCollectionPage.Prev" />
    [JsonPropertyName("prev")]
    public Linkable<ASOrderedCollectionPage>? Prev { get; set; }

    /// <inheritdoc cref="ASOrderedCollectionPage.PartOf" />
    [JsonPropertyName("partOf")]
    public Linkable<ASOrderedCollection>? PartOf { get; set; }

    /// <inheritdoc cref="ASOrderedCollectionPage.StartIndex" />
    [JsonPropertyName("startIndex")]
    [Range(0, int.MaxValue)]
    public int? StartIndex { get; set; }
}