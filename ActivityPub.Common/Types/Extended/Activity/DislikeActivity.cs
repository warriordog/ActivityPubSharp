namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor dislikes the object. 
/// </summary>
public class DislikeActivity : ASTransitiveActivity
{
    public DislikeActivity(string type = "Dislike") : base(type) {}
}