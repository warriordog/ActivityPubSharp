namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents an audio document of any kind. 
/// </summary>
public class AudioObject : DocumentObject
{
    public const string AudioType = "Audio";
    public AudioObject(string type = AudioType) : base(type) {}
}