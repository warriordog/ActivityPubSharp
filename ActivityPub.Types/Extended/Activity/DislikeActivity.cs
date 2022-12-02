namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor dislikes the object. 
/// </summary>
public class DislikeActivity : ASTransitiveActivity
{
    public const string DislikeType = "Dislike";
    public DislikeActivity(string type = DislikeType) : base(type) {}
}