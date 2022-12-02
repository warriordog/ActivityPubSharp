namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is "flagging" the object.
/// Flagging is defined in the sense common to many social platforms as reporting content as being inappropriate for any number of reasons. 
/// </summary>
public class FlagActivity : ASTransitiveActivity
{
    public const string FlagType = "Flag";
    public FlagActivity(string type = FlagType) : base(type) {}
}