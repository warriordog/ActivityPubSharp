namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is removing the object.
/// If specified, the origin indicates the context from which the object is being removed. 
/// </summary>
public class RemoveActivity : ASTransitiveActivity
{
    public const string RemoveType = "Remove";
    public RemoveActivity(string type = RemoveType) : base(type) {}
}