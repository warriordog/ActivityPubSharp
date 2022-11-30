namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor likes, recommends or endorses the object.
/// The target and origin typically have no defined meaning.
/// </summary>
public class LikeActivity : ASTransitiveActivity
{
    public const string LikeType = "Like";
    public LikeActivity(string type = LikeType) : base(type) {}
}