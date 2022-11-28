namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has listened to the object. 
/// </summary>
public class ListenActivity : ASTransitiveActivity
{
    public ListenActivity(string type = "Listen") : base(type) {}
}