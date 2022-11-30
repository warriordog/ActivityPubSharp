namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// A specialization of Accept indicating that the acceptance is tentative.
/// </summary>
public class TentativeAcceptActivity : AcceptActivity
{
    public const string TentativeAcceptType = "TentativeAccept";
    public TentativeAcceptActivity(string type = TentativeAcceptType) : base(type) {}
}