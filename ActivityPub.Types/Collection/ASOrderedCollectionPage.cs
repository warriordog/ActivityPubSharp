// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;
using static ActivityPub.Types.Collection.CollectionTypes;

namespace ActivityPub.Types.Collection;

/// <summary>
/// Used to represent ordered subsets of items from an OrderedCollection.
/// </summary>
/// <remarks>
/// Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the OrderedCollectionPage type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-orderedcollectionpage"/>
[ASTypeKey(OrderedCollectionPageType)]
public class ASOrderedCollectionPage<T> : ASCollectionPage<T>
    where T : ASObject
{

    [JsonConstructor]
    public ASOrderedCollectionPage() : this(OrderedCollectionPageType) {}

    protected ASOrderedCollectionPage(string type) : base(type) {}

    /// <summary>
    /// A non-negative integer value identifying the relative position within the logical view of a strictly ordered collection. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-startIndex"/>
    public int? StartIndex { get; set; }

    [JsonPropertyName("orderedItems")]
    public override LinkableList<T> Items { get; set; } = new();
}