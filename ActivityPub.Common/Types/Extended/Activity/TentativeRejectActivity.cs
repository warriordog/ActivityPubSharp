namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// A specialization of Reject in which the rejection is considered tentative.
/// </summary>
public class TentativeRejectActivity : RejectActivity
{
    public TentativeRejectActivity(string type = "TentativeReject") : base(type) {}
}