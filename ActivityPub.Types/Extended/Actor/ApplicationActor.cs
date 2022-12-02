namespace ActivityPub.Types.Extended.Actor;

/// <summary>
/// Describes a software application. 
/// </summary>
public class ApplicationActor : ASActor
{
    public const string ApplicationType = "Application";
    public ApplicationActor(string type = ApplicationType) : base(type) {}
}