namespace ActivityPub.Types.Internal;

/// <summary>
/// Non-extension utilities for working with Types
/// </summary>
internal static class TypeUtils
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// TODO this is broken somehow - needs extensive unit tests to identify the problem.
    /// </remarks>
    /// <param name="openType"></param>
    /// <param name="declaredType"></param>
    /// <param name="initialGenerics"></param>
    /// <param name="fallbackGenerics"></param>
    /// <returns></returns>
    internal static Type ReifyType(Type openType, Type declaredType, Type?[] initialGenerics, Type?[] fallbackGenerics)
    {
        // Fast path in case the declared type is already reified
        if (declaredType.IsAssignableToGenericType(openType))
            return declaredType;

        var genericCount = initialGenerics.Length;

        // Extract closed generics from the declared types
        var declaredGenerics = new Type?[genericCount];
        var declaredSlots = declaredType.TryGetGenericArgumentsFor(openType);
        if (declaredSlots != null)
            for (var i = 0; i < declaredSlots.Length; i++)
                if (declaredSlots[i].IsOpenGeneric())
                    declaredGenerics[i] = null;

        // Populate a list of concrete types to fill generic slots
        var genericSlots = new Type[genericCount];
        for (var i = 0; i < genericCount; i++)
        {
            genericSlots[i] =
                initialGenerics[i] // First priority - any concrete types from the original registered type
                ?? declaredGenerics[i] // Second - anything that's further narrowed by the declared type 
                ?? fallbackGenerics[i] // Next - defaults provided on the registration attribute
                ?? typeof(ASObject); // Finally - use ASObject as a generic default
        }

        // Construct the type and cache it for performance
        return openType.MakeGenericType(genericSlots);
    }
}