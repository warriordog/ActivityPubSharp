namespace ActivityPub.Common.Types;

/// <summary>
/// Required properties for all Activity Streams types.
/// </summary>
/// <remarks>
/// This is a synthetic type created to help adapt ActivityStreams to the .NET object model.
/// It does not exist in the ActivityStreams standard.
/// </remarks>
public abstract class ASType
{
    /// <summary>
    /// Short name of the definition of this type
    /// </summary>
    public string? Type { get; set; }
}