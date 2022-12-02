namespace ActivityPub.Types.Extended.Actor;

/// <summary>
/// Represents an individual person. 
/// </summary>
public class PersonActor : ASActor
{
    public const string PersonType = "Person";
    public PersonActor(string type = PersonType) : base(type) {}
}