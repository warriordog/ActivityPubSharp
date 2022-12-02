namespace ActivityPub.Types.Extended.Activity;

public class IgnoreActivity : ASTransitiveActivity
{
    public const string IgnoreType = "Ignore";
    public IgnoreActivity(string type = IgnoreType) : base(type) {}
}