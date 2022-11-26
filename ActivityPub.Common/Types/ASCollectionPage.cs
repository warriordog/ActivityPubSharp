using ActivityPub.Common.Util;

namespace ActivityPub.Common.Types;

/// <summary>
/// Used to represent distinct subsets of items from a Collection. 
/// </summary>
/// <remarks>
/// Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the CollectionPage type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-collectionpage"/>
public class ASCollectionPage : ASCollection
{
    public ASCollectionPage(string type = "CollectionPage") : base(type) {}
    
    /// <summary>
    /// In a paged Collection, indicates the next page of items. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-next"/>
    public Linkable<ASCollectionPage>? Next { get; set; }
    
    /// <summary>
    /// In a paged Collection, indicates the previous page of items. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-prev"/>
    public Linkable<ASCollectionPage>? Prev { get; set; }
    
    /// <summary>
    /// Identifies the Collection to which a CollectionPage objects items belong. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-partOf"/>
    public Linkable<ASCollection>? PartOf { get; set; }
}