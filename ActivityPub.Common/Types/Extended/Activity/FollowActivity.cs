namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is "following" the object.
/// Following is defined in the sense typically used within Social systems in which the actor is interested in any activity performed by or on the object.
/// The target and origin typically have no defined meaning. 
/// </summary>
public class FollowActivity : ASTransitiveActivity
{
    public FollowActivity(string type = "Follow") : base(type) {}
}