namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is rejecting the object.
/// The target and origin typically have no defined meaning. 
/// </summary>
public class RejectActivity : ASActivity
{
    public RejectActivity(string type = "Reject") : base(type) {}
}