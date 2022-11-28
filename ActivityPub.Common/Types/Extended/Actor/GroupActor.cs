namespace ActivityPub.Common.Types.Extended.Actor;

/// <summary>
/// Represents a formal or informal collective of Actors. 
/// </summary>
public class GroupActor : ASActor
{
    public GroupActor(string type = "Group") : base(type) {}
}