namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a Web Page. 
/// </summary>
public class PageObject : DocumentObject
{
    public const string PageType = "Page";
    public PageObject(string type = PageType) : base(type) {}
}