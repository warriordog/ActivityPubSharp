namespace ActivityPub.Common.Types.Extended.Object;

/// <summary>
/// An image document of any kind 
/// </summary>
public class ImageObject : DocumentObject
{
    public ImageObject(string type = "Image") : base(type) {}   
}