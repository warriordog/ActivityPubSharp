// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Internal;

internal class ASTypeRegistry
{
    private readonly Dictionary<string, Type> _knownTypeMap;
    public ASTypeRegistry(Dictionary<string, Type> knownTypeMap) => _knownTypeMap = knownTypeMap;

    public Type GetTypeForName(string name)
    {
        if (!_knownTypeMap.TryGetValue(name.ToLower(), out var type))
        {
            type = typeof(ASType);
        }

        return type;
    }

    public Type GetTypeForNames(string[] names)
    {
        // TODO this is a naive approach - we should handle multiple matches to find the most specific type
        foreach (var name in names)
        {
            if (_knownTypeMap.TryGetValue(name.ToLower(), out var type))
            {
                return type;
            }
        }

        return typeof(ASType);
    }

    public static ASTypeRegistry Create()
    {
        var typeMap = new Dictionary<string, Type>();

        // Find all defined types
        // TODO don't scan all assemblies
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                // Only valid on ASType + derivatives
                if (!type.IsAssignableTo(typeof(ASType)))
                    continue;

                // Its a subtype so now check for attribute
                var asObjectType = type.GetCustomAttribute<ASTypeKeyAttribute>();
                if (asObjectType != null)
                {
                    // Have to lowercase this for accurate checking 
                    var typeName = asObjectType.Type.ToLower();

                    if (typeMap.TryGetValue(typeName, out var originalType))
                    {
                        throw new ApplicationException($"Multiple classes are using AS type name {typeName}: trying to register {type} on top of {originalType}");
                    }

                    typeMap[typeName] = type;
                }
            }
        }

        return new ASTypeRegistry(typeMap);
    }
}