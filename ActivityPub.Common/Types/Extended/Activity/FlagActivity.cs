namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is "flagging" the object.
/// Flagging is defined in the sense common to many social platforms as reporting content as being inappropriate for any number of reasons. 
/// </summary>
public class FlagActivity : ASActivity
{
    public FlagActivity(string type = "Flag") : base(type) {}
}