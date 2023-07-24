// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;
using ActivityPub.Types.Json;
using InternalUtils;

namespace ActivityPub.Types.Internal.TypeInfo;

/// <summary>
/// Extracts and stores metadata for ActivityStreams types within the application.
/// </summary>
public interface IASTypeInfoCache
{
    internal JsonTypeInfo GetJsonTypeInfo<TDeclaredType>(string name) where TDeclaredType : ASType;
    
    internal bool IsKnownASType(string asTypeName);

    internal bool IsASLinkType(string type);

    /// <summary>
    /// Find and load all ActivityStreams types in a particular assembly.
    /// </summary>
    /// <param name="assembly">Assembly to load</param>
    void RegisterAssembly(Assembly assembly);

    /// <summary>
    /// Find and load all ActivityStreams types in all loaded assemblies.
    /// </summary>
    void RegisterAllAssemblies();
}

public class ASTypeInfoCache : IASTypeInfoCache
{
    private readonly HashSet<Type> _allASTypes = new();
    private readonly Dictionary<string, ASTypeInfo> _knownTypeMap = new();
    private readonly HashSet<string> _knownLinkTypes = new()
    {
        ASLink.LinkType
    };
    
    private readonly IJsonTypeInfoCache _jsonTypeInfoCache;

    public ASTypeInfoCache(IJsonTypeInfoCache jsonTypeInfoCache)
    {
        _jsonTypeInfoCache = jsonTypeInfoCache;
    }

    public JsonTypeInfo GetJsonTypeInfo<TDeclaredType>(string name) where TDeclaredType : ASType
    {
        // Narrow the type to its final, concrete form
        var realType = ReifyType<TDeclaredType>(name);

        // Get info
        return _jsonTypeInfoCache.GetForType(realType);
    }

    public bool IsKnownASType(string asTypeName) => _knownTypeMap.ContainsKey(asTypeName.ToLower());

    public bool IsASLinkType(string type)
    {
        var typeKey = type.ToLower();
        return _knownLinkTypes.Contains(typeKey);
    }

    private Type ReifyType<TDeclaredType>(string name) where TDeclaredType : ASType
    {
        if (!_knownTypeMap.TryGetValue(name.ToLower(), out var entry))
        {
            // Bail out to defaults if unknown.
            // This will blow up if TDeclaredType is abstract, open generic, or otherwise not constructable.
            return typeof(TDeclaredType);
        }

        // Delegate reification to the actual instance.
        // Inheritance FTW!
        return entry.ReifyType<TDeclaredType>();
    }

    public void RegisterAllAssemblies()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            RegisterAssembly(assembly);
        }
    }

    public void RegisterAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            // Skip if its not an AS type
            if (!type.IsAssignableTo(typeof(ASType)))
                continue;

            // Skip if we've already registered it
            if (_allASTypes.Contains(type))
                continue;

            // Pre-check this here for performance.
            // Its possible for the loop to run multiple times.
            var isASLink = type.IsAssignableTo(typeof(ASLink));
            
            // Process each attribute on the type
            var typeAttributes = type.GetCustomAttributes<ASTypeKeyAttribute>();
            foreach (var typeAttr in typeAttributes)
            {
                // Have to lowercase this for accurate checking
                var typeName = typeAttr.Type.ToLower();

                // Check for dupes
                if (_knownTypeMap.TryGetValue(typeName, out var originalType))
                    throw new ApplicationException($"Multiple classes are using AS type name {typeName}: trying to register {type} on top of {originalType}");

                // Create and cache appropriate entry for the type
                var entry = CreateASTypeInfo(type);
                _knownTypeMap[typeName] = entry;
                
                // If it derives from ASLink, then record it as an additional link type
                if (isASLink)
                {
                    _knownLinkTypes.Add(typeName);
                }
            }

            // Record it as an AS type, even if we didn't register
            _allASTypes.Add(type);
        }
    }

    private static ASTypeInfo CreateASTypeInfo(Type type)
    {
        // Open generics require a special type
        if (type.IsOpenGeneric())
            return new OpenASTypeInfo(type);
        
        return new ClosedASTypeInfo(type);

    }
}