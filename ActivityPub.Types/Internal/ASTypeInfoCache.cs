// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;
using ActivityPub.Types.AS;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Internal;

/// <summary>
///     Extracts and stores metadata for ActivityStreams types within the application.
/// </summary>
public interface IASTypeInfoCache
{
    internal bool IsKnownASType(string asTypeName);

    internal bool IsASLinkType(string type);

    /// <summary>
    ///     Finds the .NET type(s) that implement a set of AS types.
    ///     Implied types are automatically included.
    ///     Unknown types are ignored.
    /// </summary>
    /// <param name="asTypes">Types to map. Case-sensitive.</param>
    /// <returns>Set of all located types</returns>
    internal IEnumerable<Type> MapASTypes(IEnumerable<string> asTypes);

    /// <summary>
    ///     Find and load all ActivityStreams types in a particular assembly.
    /// </summary>
    /// <param name="assembly">Assembly to load</param>
    void RegisterAssembly(Assembly assembly);

    /// <summary>
    ///     Find and load all ActivityStreams types in all loaded assemblies.
    /// </summary>
    void RegisterAllAssemblies();
}

public class ASTypeInfoCache : IASTypeInfoCache
{
    private readonly HashSet<Type> _allASEntities = new();
    private readonly Dictionary<Type, HashSet<Type>> _impliedEntityMap = new();
    private readonly Dictionary<string, Type> _knownEntityMap = new();
    private readonly HashSet<string> _knownLinkTypes = new();

    public bool IsKnownASType(string asTypeName) => _knownEntityMap.ContainsKey(asTypeName.ToLower());

    public bool IsASLinkType(string type)
    {
        var typeKey = type.ToLower();
        return _knownLinkTypes.Contains(typeKey);
    }

    public IEnumerable<Type> MapASTypes(IEnumerable<string> asTypes)
    {
        var types = new HashSet<Type>();

        foreach (var asType in asTypes)
        {
            // Map AS Type to .NET Type.
            if (!_knownEntityMap.TryGetValue(asType, out var type))
                // Skip if unknown
                continue;

            // Add the type
            types.Add(type);

            // Map type to implied types
            if (!_impliedEntityMap.TryGetValue(type, out var impliedTypes))
                // Skip if there are none
                continue;

            // Add all the implied types
            foreach (var impliedType in impliedTypes)
                types.Add(impliedType);
        }

        return types;
    }

    public void RegisterAllAssemblies()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            RegisterAssembly(assembly);
    }

    public void RegisterAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            // Skip if its not an AS type
            if (!type.IsAssignableTo(typeof(ASEntity)))
                continue;

            // Skip if we've already registered it
            if (_allASEntities.Contains(type))
                continue;

            // Pre-check this here for performance.
            // Its possible for the loop to run multiple times.
            var isASLink = type.IsAssignableTo(typeof(ASLinkEntity));

            // Process each TypeKey attribute on the type
            var typeAttributes = type.GetCustomAttributes<ASTypeKeyAttribute>();
            foreach (var typeAttr in typeAttributes)
            {
                // Have to lowercase this for accurate checking
                var typeName = typeAttr.Type.ToLower();

                // Check for dupes
                if (_knownEntityMap.TryGetValue(typeName, out var originalType))
                    throw new ApplicationException($"Multiple classes are using AS type name {typeName}: trying to register {type} on top of {originalType}");

                // Register mapping
                _knownEntityMap[typeName] = type;

                // If it derives from ASLink, then record it as an additional link type
                if (isASLink)
                    _knownLinkTypes.Add(typeName);
            }

            // Register all implied types
            var impliesAttributes = type
                .GetCustomAttributes<ImpliesOtherEntityAttribute>()
                .Select(attr => attr.Type)
                .ToHashSet();
            if (impliesAttributes.Any())
                _impliedEntityMap[type] = impliesAttributes;

            // Record it as an AS type, even if we didn't register
            _allASEntities.Add(type);
        }
    }
}