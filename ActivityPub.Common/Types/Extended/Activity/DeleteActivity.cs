namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has deleted the object.
/// If specified, the origin indicates the context from which the object was deleted. 
/// </summary>
public class DeleteActivity : ASActivity
{
    public DeleteActivity(string type = "Delete") : base(type) {}
}