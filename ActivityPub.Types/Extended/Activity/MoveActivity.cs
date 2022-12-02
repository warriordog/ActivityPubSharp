namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has moved object from origin to target.
/// If the origin or target are not specified, either can be determined by context. 
/// </summary>
public class MoveActivity : ASTransitiveActivity
{
    public const string MoveType = "Move";
    public MoveActivity(string type = MoveType) : base(type) {}
}