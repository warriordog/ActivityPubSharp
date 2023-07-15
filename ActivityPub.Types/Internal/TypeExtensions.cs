namespace ActivityPub.Types.Internal;

internal static class TypeExtensions
{
    /// <summary>
    /// Determines if a concrete type can be assigned to an open generic type.
    /// Based on https://stackoverflow.com/a/1075059/
    /// </summary>
    /// <exception cref="ArgumentException">If genericType is not an open generic type</exception>
    /// <param name="concreteType">Real, concrete type</param>
    /// <param name="genericType">Open generic type</param>
    /// <returns>True if concreteType can be assigned to genericType, false otherwise</returns>
    public static bool IsAssignableToGenericType(this Type concreteType, Type genericType)
    {
        if (!genericType.IsGenericType)
            throw new ArgumentException($"Type {genericType} is not an generic", nameof(genericType));

        // Iteratively check the entire type hierarchy
        for (var type = concreteType; type != null; type = type.BaseType)
        {
            // Check if concrete type is a constructed form of genericType
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                return true;

            // Check if concreteType implements genericType
            var interfaceTypes = type.GetInterfaces();
            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
                return true;
        }

        // If nothing matched, then this is not assignable
        return false;
    }

    /// <summary>
    /// Given a specific concrete type that derives from some generic type, returns the actual types used to fill the generic.
    /// </summary>
    /// <remarks>
    /// concreteType must be assignable to genericType, but this check is skipped for performance.
    /// An exception will be thrown if the types are incompatible.
    /// To avoid this, first call <see cref="IsAssignableToGenericType"/>. 
    /// </remarks>
    /// <example>
    /// // Returns [ System.String ]
    /// GetGenericArgumentsFor(typeof(List&lt;string>), typeof(ICollection&lt;string>))
    /// </example>
    /// <exception cref="ArgumentException">If concreteType does not derive from genericType</exception>
    /// <exception cref="ArgumentException">If genericType is not an open generic type</exception>
    /// <param name="concreteType">Concrete type to extract generics from</param>
    /// <param name="genericType">Generic type containing slots to fill</param>
    /// <returns>Returns an array containing the actual type of each generic slot</returns>
    public static Type[] GetGenericArgumentsFor(this Type concreteType, Type genericType)
    {
        if (!genericType.IsGenericType)
            throw new ArgumentException($"Type {genericType} is not generic", nameof(genericType));

        // Recursively 
        Type? constructedGenericType = null;
        for (var type = concreteType; type != null; type = type.BaseType)
        {
            // Check current type directly
            if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == genericType)
            {
                constructedGenericType = type;
                break;
            }

            // Check interfaces of type
            var matchingInterface = type
                .GetInterfaces()
                .FirstOrDefault(it => it.IsConstructedGenericType && it.GetGenericTypeDefinition() == genericType);
            if (matchingInterface != null)
            {
                constructedGenericType = matchingInterface;
                break;
            }
        }

        // If constructedGenericType is still null, then concreteType is not assignable to genericType
        if (constructedGenericType == null)
            throw new ArgumentException($"Type {concreteType} is not assignable to {genericType}", nameof(concreteType));

        // Once we have the constructed type, then we can just return its generic arguments as-is.
        return constructedGenericType.GetGenericArguments();
    }

    /// <summary>
    /// Attempts to find the concrete type parameters used to fill a generic type.
    /// Returns null if the types are incompatible.
    /// </summary>
    /// <remarks>
    /// This is inefficient, but its avoids potential exceptions from <see cref="GetGenericArgumentsFor"/> when the conversion fails.
    /// </remarks>
    /// <param name="concreteType">Concrete type that extends from genericType</param>
    /// <param name="genericType">Open generic type</param>
    /// <returns>Returns array of declared type parameters on success, or null on failure.</returns>
    /// <seealso cref="GetGenericArgumentsFor"/>
    /// <seealso cref="IsAssignableToGenericType"/>
    public static Type[]? TryGetGenericArgumentsFor(this Type concreteType, Type genericType)
    {
        if (concreteType.IsAssignableToGenericType(genericType))
            return concreteType.GetGenericArgumentsFor(genericType);

        return null;
    }

    /// <summary>
    /// Checks whether the provided type is an open generic type, as opposed to closed generic or non-generic.
    /// </summary>
    /// <param name="type">Type to check</param>
    /// <returns>Returns true if open generic, closed otherwise.</returns>
    public static bool IsOpenGeneric(this Type type) => type is { IsGenericType: true, IsConstructedGenericType: false };

    /// <summary>
    /// Filters an array of types by removing any open generics.
    /// The resulting array will consist of either closed types of null.
    /// This is stable - types will not change position.
    /// </summary>
    /// <param name="types">Types to filter</param>
    public static Type?[] ToConcreteSlots(this Type?[] types)
    {
        for (var i = 0; i < types.Length; i++)
        {
            if (types[i]?.IsOpenGeneric() == true)
            {
                types[i] = null;
            }
        }

        return types;
    }
}