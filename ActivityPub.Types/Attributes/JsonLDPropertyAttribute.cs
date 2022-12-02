namespace ActivityPub.Types.Attributes;

/// <summary>
/// Identifies a property or field as being a JSON-LD property.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class JsonLDPropertyAttribute : Attribute
{
    /// <summary>
    /// If set, specifies the JSON property that maps to this field.
    /// </summary>
    public string? Name { get; }
    
    /// <summary>
    /// If set, specifies the context that defines this property.
    /// Will be used to generate the @context property during serialization, and for mapping during deserialization.
    /// Overrides the default set by <see cref="ContextAttribute"/>
    /// </summary>
    public string? Context { get; }
    
    public JsonLDPropertyAttribute(string? name = null, string? context = null)
    {
        Name = name;
        Context = context;
    }
}