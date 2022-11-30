namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has listened to the object. 
/// </summary>
public class ListenActivity : ASTransitiveActivity
{
    public const string ListenType = "Listen";
    public ListenActivity(string type = ListenType) : base(type) {}
}