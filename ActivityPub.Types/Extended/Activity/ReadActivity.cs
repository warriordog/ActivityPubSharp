namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has read the object. 
/// </summary>
public class ReadActivity : ASTransitiveActivity
{
    public const string ReadType = "Read";
    public ReadActivity(string type = ReadType) : base(type) {}
}