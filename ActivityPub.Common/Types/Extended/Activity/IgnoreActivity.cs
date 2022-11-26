namespace ActivityPub.Common.Types.Extended.Activity;

public class IgnoreActivity : ASActivity
{
    public IgnoreActivity(string type = "Ignore") : base(type) {}
}