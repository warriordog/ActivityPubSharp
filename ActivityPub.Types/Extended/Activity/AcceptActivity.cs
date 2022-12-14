namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor accepts the object.
/// The target property can be used in certain circumstances to indicate the context into which the object has been accepted. 
/// </summary>
public class AcceptActivity : ASTransitiveActivity
{
    public const string AcceptType = "Accept";
    public AcceptActivity(string type = AcceptType) : base(type) {}
}