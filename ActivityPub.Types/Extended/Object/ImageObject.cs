namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// An image document of any kind 
/// </summary>
public class ImageObject : DocumentObject
{
    public const string ImageType = "Image";
    public ImageObject(string type = ImageType) : base(type) {}   
}