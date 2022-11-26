namespace ActivityPub.Common.Types.Extended.Object;

/// <summary>
/// Represents any kind of multi-paragraph written work.
/// </summary>
public class ArticleObject : ASObject
{
    public ArticleObject(string type = "Article") : base(type) {}
}