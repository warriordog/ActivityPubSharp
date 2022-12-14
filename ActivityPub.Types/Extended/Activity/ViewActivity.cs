namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has viewed the object. 
/// </summary>
public class ViewActivity : ASTransitiveActivity
{
    public const string ViewType = "View";
    public ViewActivity(string type = ViewType) : base(type) {}
}