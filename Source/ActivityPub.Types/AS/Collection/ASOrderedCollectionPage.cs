// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Collection;

/// <summary>
///     Used to represent distinct subsets of items from an Ordered Collection.
/// </summary>
/// <remarks>
///     Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the CollectionPage type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-orderedcollectionpage" />
public class ASOrderedCollectionPage : ASOrderedCollection, IASModel<ASOrderedCollectionPage, ASOrderedCollectionPageEntity, ASOrderedCollection>
{
    /// <summary>
    ///     ActivityStreams type name for "OrderedCollectionPage" types.
    /// </summary>
    [PublicAPI]
    public const string OrderedCollectionPageType = "OrderedCollectionPage";
    static string IASModel<ASOrderedCollectionPage>.ASTypeName => OrderedCollectionPageType;

    /// <inheritdoc />
    public ASOrderedCollectionPage() => Entity = TypeMap.Extend<ASOrderedCollectionPage, ASOrderedCollectionPageEntity>();

    /// <inheritdoc />
    public ASOrderedCollectionPage(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<ASOrderedCollectionPage, ASOrderedCollectionPageEntity>(isExtending);

    /// <inheritdoc />
    public ASOrderedCollectionPage(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public ASOrderedCollectionPage(TypeMap typeMap, ASOrderedCollectionPageEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ASOrderedCollectionPage, ASOrderedCollectionPageEntity>();

    static ASOrderedCollectionPage IASModel<ASOrderedCollectionPage>.FromGraph(TypeMap typeMap) => new(typeMap, null);

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

    /// <summary>
    ///     Converts a list of objects into an ordered collection page.
    /// </summary>
    public static implicit operator ASOrderedCollectionPage(List<ASObject> collection) => new() { Items = new LinkableList<ASObject>(collection) };
}

/// <inheritdoc cref="ASOrderedCollectionPage" />
public sealed class ASOrderedCollectionPageEntity : ASEntity<ASOrderedCollectionPage, ASOrderedCollectionPageEntity>
{
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