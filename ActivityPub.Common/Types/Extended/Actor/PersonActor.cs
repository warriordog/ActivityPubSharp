namespace ActivityPub.Common.Types.Extended.Actor;

/// <summary>
/// Represents an individual person. 
/// </summary>
public class PersonActor : ASObject
{
    public PersonActor(string type = "Person") : base(type) {}
}