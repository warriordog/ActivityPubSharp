namespace ActivityPub.Common.Types.Extended.Actor;

/// <summary>
/// Represents a formal or informal collective of Actors. 
/// </summary>
public class GroupActor : ASActor
{
    public const string GroupType = "Group";
    public GroupActor(string type = GroupType) : base(type) {}
}