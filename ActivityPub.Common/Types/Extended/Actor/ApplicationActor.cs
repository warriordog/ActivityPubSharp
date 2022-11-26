namespace ActivityPub.Common.Types.Extended.Actor;

/// <summary>
/// Describes a software application. 
/// </summary>
public class ApplicationActor : ASObject
{
    public ApplicationActor(string type = "Application") : base(type) {}
}