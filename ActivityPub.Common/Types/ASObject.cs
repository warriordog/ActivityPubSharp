using ActivityPub.Common.Properties;

namespace ActivityPub.Common.Types;

/// <summary>
/// Describes an object of any kind.
/// The Object type serves as the base type for most of the other kinds of objects defined in the Activity Vocabulary, including other Core types such as Activity, IntransitiveActivity, Collection and OrderedCollection. 
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-object" />
public class ASObject : IASType
{
    public virtual string ASContext => "https://www.w3.org/ns/activitystreams#Object";
    public virtual string Type => "Object";
    
    /// <summary>
    /// Identifies a resource attached or related to an object that potentially requires special handling.
    /// The intent is to provide a model that is at least semantically similar to attachments in email. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-attachment"/>
    public LinkablePropList<AttachmentProp> Attachment { get; } = new();

    /// <summary>
    /// Identifies one or more entities to which this object is attributed.
    /// The attributed entities might not be Actors.
    /// For instance, an object might be attributed to the completion of another activity. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-attributedTo"/>
    public LinkablePropList<AttributedToProp> AttributedTo { get; } = new();

    /// <summary>
    /// Identifies one or more Objects that are part of the private secondary audience of this Object. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-bcc"/>
    public List<string> bcc { get; } = new();
    
    /// <summary>
    /// Identifies an Object that is part of the private primary audience of this Object.  
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-bto"/>
    public List<string> bto { get; } = new();
    
    /// <summary>
    /// Identifies an Object that is part of the public secondary audience of this Object.   
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-cc"/>
    public List<string> cc { get; } = new();
    
    /// <summary>
    /// Identifies the context within which the object exists or an activity was performed.
    /// </summary>
    /// <remarks>
    /// The notion of "context" used is intentionally vague.
    /// The intended function is to serve as a means of grouping objects and activities that share a common originating context or purpose.
    /// An example could be all activities relating to a common project or event.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-context"/>
    public string? Context { get; set; }
}