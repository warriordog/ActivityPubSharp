namespace ActivityPub.Common.Types;

/// <summary>
/// A Link is an indirect, qualified reference to a resource identified by a URL.
/// The fundamental model for links is established by <a href="https://tools.ietf.org/html/rfc5988">RFC5988</a>.
/// Many of the properties defined by the Activity Vocabulary allow values that are either instances of Object or Link.
/// When a Link is used, it establishes a qualified relation connecting the subject (the containing object) to the resource identified by the href.
/// Properties of the Link are properties of the reference as opposed to properties of the resource. 
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-link"/>
public class ASLink : IASType
{
    public virtual string ASContext => "https://www.w3.org/ns/activitystreams#Link";
    public virtual string Type => "Link";
}