namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is traveling to target from origin.
/// Travel is an IntransitiveObject whose actor specifies the direct object.
/// If the target or origin are not specified, either can be determined by context.
/// </summary>
public class TravelActivity : ASIntransitiveActivity
{
    public const string TravelType = "Travel";
    public TravelActivity(string type = TravelType) : base(type) {}
}