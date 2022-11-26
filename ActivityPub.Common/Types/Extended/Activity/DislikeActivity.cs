namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor dislikes the object. 
/// </summary>
public class DislikeActivity : ASActivity
{
    public DislikeActivity(string type = "Dislike") : base(type) {}
}