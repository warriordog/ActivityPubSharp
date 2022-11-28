using ActivityPub.Common.Util;

namespace ActivityPub.Common.Types;

/// <summary>
/// A Collection is a subtype of Object that represents ordered or unordered sets of Object or Link instances.
/// </summary>
/// <remarks>
/// Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the Collection type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-collection"/>
public class ASCollection : ASObject
{
    public ASCollection(string type = "Collection") : base(type) {}
    
    /// <summary>
    ///  In a paged Collection, indicates the page that contains the most recently updated member items. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-current"/>
    public Linkable<ASCollectionPage>? Current { get; set; }
    
    /// <summary>
    /// In a paged Collection, indicates the furthest preceeding page of items in the collection. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-first"/>
    public Linkable<ASCollectionPage>? First { get; set; }
    
    /// <summary>
    /// In a paged Collection, indicates the furthest proceeding page of the collection.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-attachment"/>
    public Linkable<ASCollectionPage>? Last { get; set; }

    /// <summary>
    /// Identifies the items contained in a collection.
    /// The items might be ordered or unordered. 
    /// </summary>
    /// <remarks>
    /// Can also be "orderedItems" in JSON, if this is a subclass of ASOrderedCollection.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-items"/>
    public LinkableList<ASObject> Items { get; set; } = new();

    /// <summary>
    /// A non-negative integer specifying the total number of objects contained by the logical view of the collection.
    /// This number might not reflect the actual number of items serialized within the Collection object instance. 
    /// </summary>
    public int? TotalItems { get; set; } = new();
}