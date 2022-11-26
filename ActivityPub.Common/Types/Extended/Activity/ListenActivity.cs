namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has listened to the object. 
/// </summary>
public class ListenActivity : ASActivity
{
    public ListenActivity(string type = "Listen") : base(type) {}
}