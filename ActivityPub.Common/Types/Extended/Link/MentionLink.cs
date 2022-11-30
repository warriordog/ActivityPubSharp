namespace ActivityPub.Common.Types.Extended.Link;

/// <summary>
/// A specialized Link that represents an @mention. 
/// </summary>
public class MentionLink : ASLink
{
    public const string MentionType = "Mention";
    public MentionLink(string type = MentionType) : base(type) {}
}