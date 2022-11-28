using ActivityPub.Common.Util;

namespace ActivityPub.Common.Types;

/// <summary>
/// Used to represent distinct subsets of items from a Collection. 
/// </summary>
/// <remarks>
/// Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the CollectionPage type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-collectionpage"/>
public class ASCollectionPage : ASCollection, ICollectionPage
{
    public ASCollectionPage(string type = "CollectionPage") : base(type) {}
    
    public Linkable<ASCollectionPage>? Next { get; set; }
    public Linkable<ASCollectionPage>? Prev { get; set; }
    public Linkable<ASCollection>? PartOf { get; set; }
}