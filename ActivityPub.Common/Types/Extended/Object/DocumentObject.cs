namespace ActivityPub.Common.Types.Extended.Object;

/// <summary>
/// Represents a document of any kind. 
/// </summary>
public class DocumentObject : ASObject
{
    public DocumentObject(string type = "Document") : base(type) {}
}