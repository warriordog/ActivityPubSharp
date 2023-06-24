/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
/// Used to represent distinct subsets of items from a Collection. 
/// </summary>
/// <remarks>
/// Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the CollectionPage type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-collectionpage"/>
[ASTypeKey(CollectionPageType)]
public class ASCollectionPage : ASCollection, ICollectionPage
{
    public const string CollectionPageType = "CollectionPage";

    [JsonConstructor]
    public ASCollectionPage() : this(CollectionPageType) {}

    protected ASCollectionPage(string type) : base(type) {}

    public Linkable<ASCollectionPage>? Next { get; set; }
    public Linkable<ASCollectionPage>? Prev { get; set; }
    public Linkable<ASCollection>? PartOf { get; set; }
}