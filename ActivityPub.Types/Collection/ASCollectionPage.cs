// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

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
[APType(CollectionPageType)]
public class ASCollectionPage<T> : ASCollection<T>
    where T : ASObject
{
    [JsonConstructor]
    public ASCollectionPage() : this(CollectionPageType) {}

    protected ASCollectionPage(string type) : base(type) {}

    /// <summary>
    /// In a paged Collection, indicates the next page of items. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-next"/>
    [JsonPropertyName("next")]
    public Linkable<ASCollectionPage<T>>? Next { get; set; }

    /// <summary>
    /// In a paged Collection, indicates the previous page of items. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-prev"/>
    [JsonPropertyName("prev")]
    public Linkable<ASCollectionPage<T>>? Prev { get; set; }

    /// <summary>
    /// Identifies the Collection to which a CollectionPage objects items belong. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-partOf"/>
    [JsonPropertyName("partOf")]
    public Linkable<ASCollection<T>>? PartOf { get; set; }

    public static implicit operator ASCollectionPage<T>(List<T> collection) => new() { Items = new(collection) };
    public static implicit operator ASCollectionPage<T>(T value) => new() { Items = new() { value } };
}
