namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has joined the object.
/// The target and origin typically have no defined meaning. 
/// </summary>
public class JoinActivity : ASTransitiveActivity
{
    public const string JoinType = "Join";
    public JoinActivity(string type = JoinType) : base(type) {}
}