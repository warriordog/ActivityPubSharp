// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;

namespace InternalUtils;

/// <summary>
/// Internal utilities for working with .NET types reflectively 
/// </summary>
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
    internal static bool IsAssignableToGenericType(this Type concreteType, Type genericType)
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
    internal static Type[] GetGenericArgumentsFor(this Type concreteType, Type genericType)
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
    /// Returns false/null if the types are incompatible.
    /// </summary>
    /// <remarks>
    /// This is inefficient, but its avoids potential exceptions from <see cref="GetGenericArgumentsFor"/> when the conversion fails.
    /// </remarks>
    /// <param name="concreteType">Concrete type that extends from genericType</param>
    /// <param name="genericType">Open generic type</param>
    /// <param name="arguments">Array of types used to fill genericType's slots in concreteType</param>
    /// <returns>return true if parameters were found, false otherwise</returns>
    /// <seealso cref="GetGenericArgumentsFor"/>
    /// <seealso cref="IsAssignableToGenericType"/>
    internal static bool TryGetGenericArgumentsFor(this Type concreteType, Type genericType, [NotNullWhen(true)] out Type[]? arguments)
    {
        if (concreteType.IsAssignableToGenericType(genericType))
        {
            arguments = concreteType.GetGenericArgumentsFor(genericType);
            return true;
        }

        arguments = null;
        return false;
    }

    /// <summary>
    /// Checks whether the provided type is an open generic type, as opposed to closed generic or non-generic.
    /// </summary>
    /// <param name="type">Type to check</param>
    /// <returns>Returns true if open generic, closed otherwise.</returns>
    internal static bool IsOpenGeneric(this Type type) => type is { IsGenericType: true, IsConstructedGenericType: false };

    /// <summary>
    /// Given an open generic type, returns a new type that is the result of filling all open slots with the constraint.
    /// </summary>
    /// <remarks>
    /// This is a naive implementation and may not be 100% accurate.
    /// </remarks>
    /// <param name="genericType">Type to populate. Must be an open generic type.</param>
    internal static Type GetDefaultGenericArguments(this Type genericType)
    {
        if (!genericType.IsOpenGeneric())
            throw new ArgumentException($"{genericType} is not an open generic type", nameof(genericType));
        
        var genericSlots = genericType.GetGenericArguments();
        for (var i = 0; i < genericSlots.Length; i++)
        {
            genericSlots[i] = genericSlots[i].GetGenericParameterConstraints()[0];    
        }
        return genericType.MakeGenericType(genericSlots);
    }

    /// <summary>
    /// Gets the default value for a specified type.
    /// Runtime equivalent to calling default(T).
    /// </summary>
    /// <remarks>
    /// Based on https://stackoverflow.com/a/3195792
    /// </remarks>
    /// <param name="type">Type of value</param>
    /// <returns>Returns the default value of the type</returns>
    internal static object? GetDefaultValue(this Type type)
    {
        // The only case where it matters are non-nullable value types
        if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
            return Activator.CreateInstance(type);
        
        // For everything else, the correct value is just null
        return null;
    }
}