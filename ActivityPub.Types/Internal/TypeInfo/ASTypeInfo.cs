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

// TODO I think we don't need 90% of this
// We can't even enter this code path without a fully concrete type.
// I think we only get the open one because we *replace* the closed type with the open one.
// The only case I can think of is something like this:
//   Declared:     ASCollection<ASObject> ----------+
//   Returned:     ASOrderedCollection              |
//   Lookup gets:  ASOrderedCollection<T>           |
//   What we want: ASOrderedCollection<ASObject> <--+
// I think we can just use TryGetGenericArgumentsFor() instead.
// We should probably still cache for performance, but it would remove most of this janky code.

/// <summary>
/// An ActivityStreams type with open generic slots - it must be reified before use.
/// Internally caches all reified subtypes for performance.
/// </summary>
internal class OpenASTypeInfo : ASTypeInfo
{
    private readonly Dictionary<Type, Type> _subtypeCache = new();
    private readonly Type?[] _initialGenerics;
    private readonly Type?[] _fallbackGenerics;

    public OpenASTypeInfo(Type registeredType, Type?[] initialGenerics, Type?[] fallbackGenerics) : base(registeredType)
    {
        if (!registeredType.IsOpenGeneric())
            throw new ArgumentException($"Type {registeredType} is not an open generic", nameof(registeredType));

        _initialGenerics = initialGenerics;
        _fallbackGenerics = fallbackGenerics;
    }

    public override Type ReifyType<TDeclaredType>()
    {
        var declaredType = typeof(TDeclaredType);

        // Check for cache, and create if missing
        if (!_subtypeCache.TryGetValue(declaredType, out var reifiedType))
        {
            reifiedType = TypeUtils.ReifyType(RegisteredType, declaredType, _initialGenerics, _fallbackGenerics);
            _subtypeCache[declaredType] = reifiedType;
        }

        return reifiedType;
    }
}