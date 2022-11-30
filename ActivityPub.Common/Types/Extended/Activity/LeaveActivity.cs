namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has left the object.
/// The target and origin typically have no meaning.
/// </summary>
public class LeaveActivity : ASTransitiveActivity
{
    public const string LeaveType = "Leave";
    public LeaveActivity(string type = LeaveType) : base(type) {}
}