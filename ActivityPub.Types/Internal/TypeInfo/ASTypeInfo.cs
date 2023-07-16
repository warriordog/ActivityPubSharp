// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using InternalUtils;

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
    public override Type ReifyType<TDeclaredType>()
    {
        var declaredType = typeof(TDeclaredType);
        
        // Case 1 - TDeclaredType derives from RegisteredType
        if (declaredType.IsAssignableTo(RegisteredType))
            return declaredType;
        
        // Case 2 - they are equal OR RegisteredType derives from TDeclaredType
        if (RegisteredType.IsAssignableTo(declaredType))
            return RegisteredType;
        
        // Case 3 - they are not related!
        // Time to blow up :(
        throw new ArgumentException($"registered type {RegisteredType} and declared type {declaredType} don't relate to each other");
    }

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
    
    // genericType is the one returned by ASType lookup.
    // declaredType is taken from a concrete POCO, so it should always be constructable.
    private static Type PopulateGenericType(Type genericType, Type declaredType)
    {
        // Sanity checks
        if (!genericType.IsOpenGeneric())
            throw new ArgumentException($"{genericType} is not an open generic type - it must be open", nameof(genericType));
        if (declaredType.IsOpenGeneric())
            throw new ArgumentException($"{declaredType} is an open generic type - it must be closed", nameof(declaredType));
        
        // Case 1 (easy) - genericType is less specific than declaredType.
        // We can just keep the declared type.
        if (declaredType.IsAssignableToGenericType(genericType))
            return declaredType;
        
        // Case 3 - genericType is more specific than declaredType.
        // We can only infer generic slots from the constraints.
        if (GenericIsAssignableToDeclared(genericType, declaredType))
            return genericType.GetDefaultGenericArguments();
        
        // Case 2 (bad) - there is no relation between genericType and declaredType.
        // This is probably an error in JSON or the type definition; nothing we can do.
        throw new ArgumentException($"generic type {genericType} and declared type {declaredType} don't relate to each other");
    }
    
    private static bool GenericIsAssignableToDeclared(Type genericType, Type declaredType)
    {
        // Declared type could be a constructed generic type.
        // If so, we need to de-construct it for comparison.
        if (declaredType.IsGenericType)
        {
            var openDeclaredType = declaredType.GetGenericTypeDefinition();
            return genericType.IsAssignableToGenericType(openDeclaredType);
        }
        
        // Otherwise, we check for direct inheritance
        return genericType.IsAssignableTo(declaredType);
    }
}