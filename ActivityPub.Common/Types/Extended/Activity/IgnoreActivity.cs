namespace ActivityPub.Common.Types.Extended.Activity;

public class IgnoreActivity : ASTransitiveActivity
{
    public IgnoreActivity(string type = "Ignore") : base(type) {}
}