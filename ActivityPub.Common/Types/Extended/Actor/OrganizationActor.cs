namespace ActivityPub.Common.Types.Extended.Actor;

/// <summary>
/// Represents an organization. 
/// </summary>
public class OrganizationActor : ASObject
{
    public OrganizationActor(string type = "Organization") : base(type) {}
}