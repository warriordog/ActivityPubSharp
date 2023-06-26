/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

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
[ASTypeKey(CollectionPageType)]
public class ASCollectionPage<T> : ASUnpagedCollection<T>
    where T : ASObject
{
    [JsonConstructor]
    public ASCollectionPage() : this(CollectionPageType) {}

    protected ASCollectionPage(string type) : base(type) {}

    /// <summary>
    /// In a paged Collection, indicates the next page of items. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-next"/>
    public Linkable<ASCollectionPage<T>>? Next { get; set; }

    /// <summary>
    /// In a paged Collection, indicates the previous page of items. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-prev"/>
    public Linkable<ASCollectionPage<T>>? Prev { get; set; }

    /// <summary>
    /// Identifies the Collection to which a CollectionPage objects items belong. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-partOf"/>
    public Linkable<ASCollection<T>>? PartOf { get; set; }
}