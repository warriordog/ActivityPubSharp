namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has left the object.
/// The target and origin typically have no meaning.
/// </summary>
public class LeaveActivity : ASActivity
{
    public LeaveActivity(string type = "Leave") : base(type) {}
}