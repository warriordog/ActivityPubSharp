namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has joined the object.
/// The target and origin typically have no defined meaning. 
/// </summary>
public class JoinActivity : ASActivity
{
    public JoinActivity(string type = "Join") : base(type) {}
}