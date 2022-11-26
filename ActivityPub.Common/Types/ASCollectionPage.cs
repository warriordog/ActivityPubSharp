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
    public override string ASContext => "https://www.w3.org/ns/activitystreams#CollectionPage";
    public override string Type => "CollectionPage";
}