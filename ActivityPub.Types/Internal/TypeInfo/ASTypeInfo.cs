namespace ActivityPub.Types.Internal.TypeInfo;

/// <summary>
/// Metadata singleton for a specific ActivityStreams type
/// </summary>
internal abstract class ASTypeInfo
{
    public Type RegisteredType { get; }
    protected ASTypeInfo(Type registeredType) => RegisteredType = registeredType;

    public abstract Type ReifyType<TDeclaredType>() where TDeclaredType : ASType;
}

/// <summary>
/// An ActivityStreams type that is closed - meaning that it has no open generic slots.
/// </summary>
internal class ClosedASTypeInfo : ASTypeInfo
{
    // TODO should we validate TDeclaredType?
    public override Type ReifyType<TDeclaredType>() => RegisteredType;

    public ClosedASTypeInfo(Type registeredType) : base(registeredType) {}
}

/// <summary>
/// An ActivityStreams type with open generic slots - it must be reified before use.
/// Internally caches all reified subtypes for performance.
/// </summary>
internal class OpenASTypeInfo : ASTypeInfo
{
    private readonly Dictionary<Type, Type> _subtypeCache = new();

    public OpenASTypeInfo(Type registeredType) : base(registeredType)
    {
        if (!registeredType.IsOpenGeneric())
            throw new ArgumentException($"Type {registeredType} is not an open generic", nameof(registeredType));
    }

    public override Type ReifyType<TDeclaredType>()
    {
        var declaredType = typeof(TDeclaredType);

        // Check if its in the cache, create it if not
        if (!_subtypeCache.TryGetValue(declaredType, out var reifiedType))
        {
            reifiedType = PopulateGenericType(RegisteredType, declaredType);
            if (!reifiedType.IsConstructedGenericType)
                throw new ApplicationException($"BUG: {RegisteredType} is still generic after reifying against {declaredType}");
            
            _subtypeCache[declaredType] = reifiedType;
        }
        
        return reifiedType;
    }
    
    private static Type PopulateGenericType(Type genericType, Type declaredType)
    {
        // Take an alternate path for non-generic base
        if (!declaredType.IsGenericType)
            return PopulateDefaultGenerics(genericType);
        
        
        // Put the checks here for performance
        if (!declaredType.IsConstructedGenericType)
            throw new ArgumentException("declared type must be a closed generic type", nameof(declaredType));
        if (!declaredType.IsAssignableToGenericType(genericType))
            throw new ArgumentException($"declared type {declaredType} must be assignable to generic type {genericType}");

        // Copy the generic type from declaredType to openType
        var genericSlots = declaredType.GetGenericArgumentsFor(genericType);
        return genericType.MakeGenericType(genericSlots);
    }

    private static Type PopulateDefaultGenerics(Type genericType)
    {
        var genericSlots = genericType.GetGenericArguments();
        for (var i = 0; i < genericSlots.Length; i++)
        {
            // TODO this probably isn't accurate
            genericSlots[i] = genericSlots[i].GetGenericParameterConstraints()[0];    
        }
        return genericType.MakeGenericType(genericSlots);
    }
}