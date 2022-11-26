namespace ActivityPub.Common.Types.Extended.Object;

/// <summary>
/// Represents a video document of any kind. 
/// </summary>
public class VideoObject : DocumentObject
{
    public VideoObject(string type = "Video") : base(type) {}
}