// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;
using static ActivityPub.Types.Collection.CollectionTypes;

namespace ActivityPub.Types.Collection;

/// <summary>
/// Used to represent distinct subsets of items from a Collection. 
/// </summary>
/// <remarks>
/// Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the CollectionPage type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-collectionpage"/>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-orderedcollectionpage"/>
[ASTypeKey(CollectionPageType)]
public class ASCollectionPage<T> : ASCollection<T>
    where T : ASObject
{
    private ASCollectionPageEntity<T> Entity { get; }

    
    public ASCollectionPage() => Entity = new ASCollectionPageEntity<T>(TypeMap);
    public ASCollectionPage(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ASCollectionPageEntity<T>>();

    
    /// <summary>
    /// In a paged Collection, indicates the next page of items. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-next"/>
    public Linkable<ASCollectionPage<T>>? Next 
    {
        get => Entity.Next;
        set => Entity.Next = value;
    }

    /// <summary>
    /// In a paged Collection, indicates the previous page of items. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-prev"/>
    public Linkable<ASCollectionPage<T>>? Prev
    {
        get => Entity.Prev;
        set => Entity.Prev = value;
    }

    /// <summary>
    /// Identifies the Collection to which a CollectionPage objects items belong. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-partOf"/>
    public Linkable<ASCollection<T>>? PartOf
    {
        get => Entity.PartOf;
        set => Entity.PartOf = value;
    }
    
    /// <summary>
    /// A non-negative integer value identifying the relative position within the logical view of a strictly ordered collection.
    /// This is only valid on OrderedCollectionPage, but it exists on unordered CollectionPage due to technical limitations.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-startIndex"/>
    public int? StartIndex
    {
        get => Entity.StartIndex;
        set => Entity.StartIndex = value;
    }

    
    public static implicit operator ASCollectionPage<T>(List<T> collection) => new() { Items = new(collection) };
    public static implicit operator ASCollectionPage<T>(T value) => new() { Items = new() { value } };
}


/// <inheritdoc cref="ASCollectionPage{T}"/>
[ASTypeKey(CollectionPageType)]
[ASTypeKey(OrderedCollectionPageType)]
public sealed class ASCollectionPageEntity<T> : ASBase
    where T : ASObject
{
    public ASCollectionPageEntity(TypeMap typeMap) : base(CollectionType, typeMap) {}

    /// <inheritdoc cref="ASCollectionPage{T}.Next"/>
    [JsonPropertyName("next")]
    public Linkable<ASCollectionPage<T>>? Next { get; set; }

    /// <inheritdoc cref="ASCollectionPage{T}.Prev"/>
    [JsonPropertyName("prev")]
    public Linkable<ASCollectionPage<T>>? Prev { get; set; }

    /// <inheritdoc cref="ASCollectionPage{T}.PartOf"/>
    [JsonPropertyName("partOf")]
    public Linkable<ASCollection<T>>? PartOf { get; set; }
    
    /// <inheritdoc cref="ASCollectionPage{T}.StartIndex"/>
    [JsonPropertyName("startIndex")]
    [Range(0, int.MaxValue)]
    public int? StartIndex { get; set; }
}