/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
/// Used to represent distinct subsets of items from a Collection. 
/// </summary>
/// <remarks>
/// Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the CollectionPage type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-collectionpage"/>
public class ASCollectionPage : ASCollection, ICollectionPage
{
    public const string CollectionPageType = "CollectionPage";
    public ASCollectionPage(string type = CollectionPageType) : base(type) {}
    
    public Linkable<ASCollectionPage>? Next { get; set; }
    public Linkable<ASCollectionPage>? Prev { get; set; }
    public Linkable<ASCollection>? PartOf { get; set; }
}