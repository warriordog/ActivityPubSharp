namespace ActivityPub.Common.Types;

/// <summary>
/// An Activity is a subtype of Object that describes some form of action that may happen, is currently happening, or has already happened.
/// The Activity type itself serves as an abstract base type for all types of activities.
/// It is important to note that the Activity type itself does not carry any specific semantics about the kind of action being taken. 
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-activity"/>
public class ASActivity : ASObject
{
    public override string ASContext => "https://www.w3.org/ns/activitystreams#Activity";
    public override string Type => "Activity";
}