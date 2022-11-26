namespace ActivityPub.Common.Properties;

/// <summary>
/// Identifies one or more entities that represent the total population of entities for which the object can considered to be relevant. 
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-audience"/>
public class AudienceProp
{
    public required string Type { get; set; }
    public required string Name { get; set; }
}