namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has read the object. 
/// </summary>
public class ReadActivity : ASActivity
{
    public ReadActivity(string type = "Read") : base(type) {}
}