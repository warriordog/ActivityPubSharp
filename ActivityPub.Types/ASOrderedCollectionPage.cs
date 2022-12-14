using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
/// Used to represent ordered subsets of items from an OrderedCollection.
/// </summary>
/// <remarks>
/// Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the OrderedCollectionPage type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-orderedcollectionpage"/>
public class ASOrderedCollectionPage : ASOrderedCollection, ICollectionPage
{
    public const string OrderedCollectionPageType = "OrderedCollectionPage";
    public ASOrderedCollectionPage(string type = OrderedCollectionPageType) : base(type) {}
    
    /// <summary>
    /// A non-negative integer value identifying the relative position within the logical view of a strictly ordered collection. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-startIndex"/>
    public int? StartIndex { get; set; }
    
    public Linkable<ASCollectionPage>? Next { get; set; }
    public Linkable<ASCollectionPage>? Prev { get; set; }
    public Linkable<ASCollection>? PartOf { get; set; }
}