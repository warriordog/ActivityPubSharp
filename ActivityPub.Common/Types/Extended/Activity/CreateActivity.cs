namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has created the object.
/// </summary>
public class CreateActivity : ASActivity
{
    public CreateActivity(string type = "Create") : base(type) {}
}