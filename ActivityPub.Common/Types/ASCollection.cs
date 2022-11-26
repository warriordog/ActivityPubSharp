using ActivityPub.Common.Properties;

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
    public ASCollection() => Type ??= "Collection";
    
    public LinkableProp<ASLink>? Current { get; set; }
    public LinkableProp<ASLink>? First { get; set; }
}