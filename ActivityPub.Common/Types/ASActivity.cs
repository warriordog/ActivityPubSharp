using ActivityPub.Common.Util;

namespace ActivityPub.Common.Types;

/// <summary>
/// An Activity is a subtype of Object that describes some form of action that may happen, is currently happening, or has already happened.
/// The Activity type itself serves as an abstract base type for all types of activities.
/// It is important to note that the Activity type itself does not carry any specific semantics about the kind of action being taken. 
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-activity"/>
public class ASActivity : ASObject
{
    public const string ActivityType = "Activity";
    public ASActivity(string type = ActivityType) : base(type) {}

    /// <summary>
    /// Describes one or more entities that either performed or are expected to perform the activity.
    /// Any single activity can have multiple actors.
    /// The actor MAY be specified using an indirect Link. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-attachment"/>
    public LinkableList<ASObject> Actor { get; set; } = new();
    
    /// <summary>
    /// Identifies one or more objects used (or to be used) in the completion of an Activity. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-instrument"/>
    public Linkable<ASObject>? Instrument { get; set; }
    
    /// <summary>
    /// Describes an indirect object of the activity from which the activity is directed.
    /// The precise meaning of the origin is the object of the English preposition "from".
    /// For instance, in the activity "John moved an item to List B from List A", the origin of the activity is "List A". 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-origin"/>
    public Linkable<ASObject>? Origin { get; set; }
    
    /// <summary>
    /// Describes the result of the activity.
    /// For instance, if a particular action results in the creation of a new resource, the result property can be used to describe that new resource. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-result"/>
    public Linkable<ASObject>? Result { get; set; }
    
    /// <summary>
    /// Describes the indirect object, or target, of the activity.
    /// The precise meaning of the target is largely dependent on the type of action being described but will often be the object of the English preposition "to".
    /// For instance, in the activity "John added a movie to his wishlist", the target of the activity is John's wishlist.
    /// An activity can have more than one target. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-target"/>
    public Linkable<ASObject>? Target { get; set; }
}