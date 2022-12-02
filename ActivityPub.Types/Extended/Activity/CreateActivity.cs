namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has created the object.
/// </summary>
public class CreateActivity : ASTransitiveActivity
{
    public const string CreateType = "Create";
    public CreateActivity(string type = CreateType) : base(type) {}
}