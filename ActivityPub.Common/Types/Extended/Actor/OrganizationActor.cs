namespace ActivityPub.Common.Types.Extended.Actor;

/// <summary>
/// Represents an organization. 
/// </summary>
public class OrganizationActor : ASActor
{
    public const string OrganizationType = "Organization";
    public OrganizationActor(string type = OrganizationType) : base(type) {}
}