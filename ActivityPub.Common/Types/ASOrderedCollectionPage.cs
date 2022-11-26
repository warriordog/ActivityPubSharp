namespace ActivityPub.Common.Types;

/// <summary>
/// Used to represent ordered subsets of items from an OrderedCollection.
/// </summary>
/// <remarks>
/// Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the OrderedCollectionPage type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-orderedcollectionpage"/>
public class ASOrderedCollectionPage : ASOrderedCollection
{
    public ASOrderedCollectionPage() => Type ??= "OrderedCollectionPage";
}