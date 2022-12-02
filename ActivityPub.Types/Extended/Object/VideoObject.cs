namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a video document of any kind. 
/// </summary>
public class VideoObject : DocumentObject
{
    public const string VideoType = "Video";
    public VideoObject(string type = VideoType) : base(type) {}
}