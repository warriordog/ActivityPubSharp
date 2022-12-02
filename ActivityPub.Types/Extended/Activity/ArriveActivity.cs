namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// An IntransitiveActivity that indicates that the actor has arrived at the location.
/// The origin can be used to identify the context from which the actor originated.
/// The target typically has no defined meaning. 
/// </summary>
public class ArriveActivity : ASIntransitiveActivity
{
    public const string ArriveType = "Arrive";
    public ArriveActivity(string type = ArriveType) : base(type) {}
}