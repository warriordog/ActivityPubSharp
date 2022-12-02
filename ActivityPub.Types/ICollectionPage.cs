using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
/// Base for any type that can act as a collection page
/// </summary>
/// <remarks>
/// This is a synthetic type included to handle the fact the OrderedCollectionPage extends from both CollectionPage and OrderedCollection
/// </remarks>
public interface ICollectionPage
{
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