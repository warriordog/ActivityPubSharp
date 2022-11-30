namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is calling the target's attention the object.
/// The origin typically has no defined meaning. 
/// </summary>
public class AnnounceActivity : ASTransitiveActivity
{
    public const string AnnounceType = "Announce";
    public AnnounceActivity(string type = AnnounceType) : base(type) {}
}