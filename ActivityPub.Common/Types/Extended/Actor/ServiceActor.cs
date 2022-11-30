namespace ActivityPub.Common.Types.Extended.Actor;

/// <summary>
/// Represents a service of any kind.
/// </summary>
public class ServiceActor : ASActor
{
    public const string ServiceType = "Service";
    public ServiceActor(string type = ServiceType) : base(type) {}
}