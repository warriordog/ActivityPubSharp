namespace ActivityPub.Types.Attributes;

/// <summary>
/// Specifies the context to use for a type.
/// Can be overridden per-property by <see cref="JsonLDPropertyAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ContextAttribute : Attribute
{
    public string ContextUrl { get; }
    public ContextAttribute(string contextUrl) => ContextUrl = contextUrl;
}