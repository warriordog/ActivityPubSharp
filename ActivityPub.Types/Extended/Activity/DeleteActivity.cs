namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has deleted the object.
/// If specified, the origin indicates the context from which the object was deleted. 
/// </summary>
public class DeleteActivity : ASTransitiveActivity
{
    public const string DeleteType = "Delete";
    public DeleteActivity(string type = DeleteType) : base(type) {}
}