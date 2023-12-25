// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Runtime.CompilerServices;
using ActivityPub.Types.AS;

namespace ActivityPub.Types.Internal;

internal static class ASNameTree
{
    private static readonly Dictionary<string, string> NameBaseMap = new();
    private static readonly Dictionary<string, HashSet<string>> NameDescendentsMap = new();

    public static void Add(string? asTypeName, string? baseTypeName)
    {
        if (asTypeName == null || baseTypeName == null)
            return;

        // When adding a new entry, we may be filling in a gap.
        // If so, then we need to "promote" the existing descendents up to the base(s).
        if (
            NameBaseMap.TryAdd(asTypeName, baseTypeName) &&
            NameDescendentsMap.TryGetValue(asTypeName, out var existingDescendents))
        {
            foreach (var descendentName in existingDescendents)
            {
                AddRecursive(baseTypeName, descendentName);
            }
        }
        
        // We have to recursively update all base types
        AddRecursive(baseTypeName, asTypeName);
    }
    
    private static void AddRecursive(string? asTypeName, string descendentTypeName)
    {
        if (asTypeName == null)
            return;
        
        do
        {
            if (!NameDescendentsMap.TryGetValue(asTypeName, out var derivedTypes))
            {
                derivedTypes = new HashSet<string>();
                NameDescendentsMap[asTypeName] = derivedTypes;
            }

            derivedTypes.Add(descendentTypeName);
        } while (NameBaseMap.TryGetValue(asTypeName, out asTypeName));
    }

    public static HashSet<string>? GetDerivedTypesFor(string? asTypeName)
    {
        if (asTypeName != null && NameDescendentsMap.TryGetValue(asTypeName, out var derivedTypes))
            return derivedTypes;

        return null;
    }
    
    
    /// <summary>
    ///     Static constructor to ensure that all AS types have been statically initialized.
    ///     The CLR will not automatically call static initializers in interfaces.
    ///     https://stackoverflow.com/questions/5299737/static-constructor-on-a-net-interface-is-not-run
    ///     https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.runtimehelpers.runclassconstructor?view=net-8.0
    /// </summary>
    static ASNameTree()
    {
        var interfaceTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsAssignableTo(typeof(ASType)))
            .SelectMany(t => t.GetInterfaces());
        
        foreach (var type in interfaceTypes)
        {
            RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        }
    }
}