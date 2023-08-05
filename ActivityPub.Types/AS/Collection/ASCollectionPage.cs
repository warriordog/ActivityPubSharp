// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.AS.Collection;

/// <summary>
///     Used to represent distinct subsets of items from a Collection.
/// </summary>
/// <remarks>
///     Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the CollectionPage type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-collectionpage" />
public class ASCollectionPage : ASCollection
{
    public ASCollectionPage() => Entity = new ASCollectionPageEntity { TypeMap = TypeMap };
    public ASCollectionPage(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ASCollectionPageEntity>();
    private ASCollectionPageEntity Entity { get; }


    /// <summary>
    ///     In a paged Collection, indicates the next page of items.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-next" />
    public Linkable<ASCollectionPage>? Next
    {
        get => Entity.Next;
        set => Entity.Next = value;
    }

    /// <summary>
    ///     In a paged Collection, indicates the previous page of items.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-prev" />
    public Linkable<ASCollectionPage>? Prev
    {
        get => Entity.Prev;
        set => Entity.Prev = value;
    }

    /// <summary>
    ///     Identifies the Collection to which a CollectionPage objects items belong.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-partOf" />
    public Linkable<ASCollection>? PartOf
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

    public static implicit operator ASCollectionPage(List<ASObject> collection) => new() { Items = new LinkableList<ASObject>(collection) };
}

/// <inheritdoc cref="ASCollectionPage" />
[ASTypeKey(CollectionPageType)]
[ImpliesOtherEntity(typeof(ASCollectionEntity))]
public sealed class ASCollectionPageEntity : ASEntity<ASCollectionPage>
{
    public const string CollectionPageType = "CollectionPage";
    public override string ASTypeName => CollectionPageType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASCollectionEntity.CollectionType
    };

    /// <inheritdoc cref="ASCollectionPage.Next" />
    [JsonPropertyName("next")]
    public Linkable<ASCollectionPage>? Next { get; set; }

    /// <inheritdoc cref="ASCollectionPage.Prev" />
    [JsonPropertyName("prev")]
    public Linkable<ASCollectionPage>? Prev { get; set; }

    /// <inheritdoc cref="ASCollectionPage.PartOf" />
    [JsonPropertyName("partOf")]
    public Linkable<ASCollection>? PartOf { get; set; }

    /// <inheritdoc cref="ASCollectionPage.StartIndex" />
    [JsonPropertyName("startIndex")]
    [Range(0, int.MaxValue)]
    public int? StartIndex { get; set; }
}