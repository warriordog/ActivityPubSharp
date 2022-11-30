namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is blocking the object.
/// Blocking is a stronger form of Ignore.
/// The typical use is to support social systems that allow one user to block activities or content of other users.
/// The target and origin typically have no defined meaning. 
/// </summary>
public class BlockActivity : IgnoreActivity
{
    public const string BlockType = "BlockActivity";
    public BlockActivity(string type = BlockType) : base(type) {}
}