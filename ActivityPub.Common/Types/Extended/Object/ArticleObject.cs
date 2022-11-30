namespace ActivityPub.Common.Types.Extended.Object;

/// <summary>
/// Represents any kind of multi-paragraph written work.
/// </summary>
public class ArticleObject : ASObject
{
    public const string ArticleType = "Article";
    public ArticleObject(string type = ArticleType) : base(type) {}
}