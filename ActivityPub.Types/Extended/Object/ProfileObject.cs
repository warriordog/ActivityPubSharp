namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// A Profile is a content object that describes another Object, typically used to describe Actor Type objects.
/// The describes property is used to reference the object being described by the profile. 
/// </summary>
public class ProfileObject : ASObject
{
    public const string ProfileType = "Profile";
    public ProfileObject(string type = ProfileType) : base(type) {}
    
    /// <summary>
    /// On a Profile object, the describes property identifies the object described by the Profile.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-describes"/>
    public ASObject? Describes { get; set; }
}