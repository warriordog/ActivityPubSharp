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
    public bool IsKnownASType(string asTypeName);

    public bool IsASLinkType(string type);

    /// <summary>
    ///     Finds the .NET type(s) that implement a set of AS types.
    ///     Implied types are automatically included.
    ///     Unknown types are ignored.
    /// </summary>
    /// <param name="asTypes">Types to map. Case-sensitive.</param>
    /// <param name="mappedTypes"></param>
    /// <param name="unmappedTypes"></param>
    /// <returns>Set of all located types</returns>
    internal void MapASTypes(IEnumerable<string> asTypes, out HashSet<Type> mappedTypes, out HashSet<string> unmappedTypes);

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

    public bool IsKnownASType(string asTypeName) => _knownEntityMap.ContainsKey(asTypeName);

    public bool IsASLinkType(string type) => _knownLinkTypes.Contains(type);

    public void MapASTypes(IEnumerable<string> asTypes, out HashSet<Type> mappedTypes, out HashSet<string> unmappedTypes)
    {
        mappedTypes = new HashSet<Type>();
        unmappedTypes = new HashSet<string>();

        foreach (var asType in asTypes)
        {
            // Map AS Type to .NET Type.
            if (_knownEntityMap.TryGetValue(asType, out var type))
                AddType(mappedTypes, type);

            // Record unknown
            else
                unmappedTypes.Add(asType);
        }
    }

    private void AddType(HashSet<Type> types, Type type)
    {
        // Check + add.
        // If already present, then skip next steps.
        // This is both a performance boost + minimizes impact of a circular dependency bug.
        if (!types.Add(type))
            return;

        // Map type to implied types.
        // Skip if there are none.
        if (!_impliedEntityMap.TryGetValue(type, out var impliedTypes))
            return;

        // Add all the implied types
        foreach (var impliedType in impliedTypes)
            AddType(types, impliedType);
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
            var isASLink = type.IsAssignableTo(typeof(ILinkEntity));

            // Process each APConvertible attribute on the type
            var convertibleAttrs = type.GetCustomAttributes<APConvertibleAttribute>();
            foreach (var convertibleAttr in convertibleAttrs)
            {
                var typeName = convertibleAttr.Type;

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