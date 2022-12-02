namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// A specialization of Reject in which the rejection is considered tentative.
/// </summary>
public class TentativeRejectActivity : RejectActivity
{
    public const string TentativeRejectType = "TentativeReject";
    public TentativeRejectActivity(string type = TentativeRejectType) : base(type) {}
}