namespace ActivityPub.Common.Types;

/// <summary>
/// A subtype of Collection in which members of the logical collection are assumed to always be strictly ordered.
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-orderedcollection"/>
public class ASOrderedCollection : ASCollection
{
    public const string OrderedCollectionType = "OrderedCollection";
    public ASOrderedCollection(string type = OrderedCollectionType) : base(type) {}
}