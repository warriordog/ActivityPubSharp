// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ActivityPub.Types.Internal;

/// <summary>
///     Internal utilities for working with .NET types reflectively
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    ///     Determines if a concrete type can be assigned to an open generic type.
    ///     Based on https://stackoverflow.com/a/1075059/
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
    ///     Given a specific concrete type that derives from some generic type, returns the actual types used to fill the generic.
    /// </summary>
    /// <remarks>
    ///     concreteType must be assignable to genericType, but this check is skipped for performance.
    ///     An exception will be thrown if the types are incompatible.
    ///     To avoid this, first call <see cref="IsAssignableToGenericType" />.
    /// </remarks>
    /// <example>
    ///     // Returns [ System.String ]
    ///     GetGenericArgumentsFor(typeof(List&lt;string>), typeof(ICollection&lt;string>))
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
    ///     Attempts to find the concrete type parameters used to fill a generic type.
    ///     Returns false/null if the types are incompatible.
    /// </summary>
    /// <remarks>
    ///     This is inefficient, but its avoids potential exceptions from <see cref="GetGenericArgumentsFor" /> when the conversion fails.
    /// </remarks>
    /// <param name="concreteType">Concrete type that extends from genericType</param>
    /// <param name="genericType">Open generic type</param>
    /// <param name="arguments">Array of types used to fill genericType's slots in concreteType</param>
    /// <returns>return true if parameters were found, false otherwise</returns>
    /// <seealso cref="GetGenericArgumentsFor" />
    /// <seealso cref="IsAssignableToGenericType" />
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

    internal static MethodInfo GetRequiredMethod(this Type type, string methodName, BindingFlags bindingFlags = BindingFlags.Default)
    {
        var method = type.GetMethod(methodName, bindingFlags);
        if (method == null)
            throw new MissingMethodException(type.FullName, methodName);

        return method;
    }

    internal static bool TryMakeGenericType(this Type type, [NotNullWhen(true)] out Type? genericType, params Type[] arguments)
    {
        try
        {
            genericType = type.MakeGenericType(arguments);
            return true;
        }
        
        // This is actually the best way: https://stackoverflow.com/a/4864565
        catch (ArgumentException)
        {
            genericType = null;
            return false;
        }
    }
}