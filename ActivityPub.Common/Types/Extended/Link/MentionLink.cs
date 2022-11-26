namespace ActivityPub.Common.Types.Extended.Link;

/// <summary>
/// A specialized Link that represents an @mention. 
/// </summary>
public class MentionLink : ASLink
{
    public MentionLink(string type = "Mention") : base(type) {}
}