using System.Reflection;

namespace ActivityPub.Types.Internal.TypeInfo;

/// <summary>
/// Cached details about a particular JSON property
/// </summary>
public class JsonPropertyInfo
{
    public required PropertyInfo Property { get; init; }
    public required string Name { get; init; }
    public required bool IsRequired { get; init; }
}