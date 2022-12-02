namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a document of any kind. 
/// </summary>
public class DocumentObject : ASObject
{
    public const string DocumentType = "Document";
    public DocumentObject(string type = DocumentType) : base(type) {}
}