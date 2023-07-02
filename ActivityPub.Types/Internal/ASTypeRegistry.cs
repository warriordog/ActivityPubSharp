// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Internal;

// TODO make this a service with DI
internal class ASTypeRegistry
{
    private readonly Dictionary<string, TypeRegistryEntry> _knownTypeMap;
    private ASTypeRegistry(Dictionary<string, TypeRegistryEntry> knownTypeMap) => _knownTypeMap = knownTypeMap;

    public Type ReifyType<TDeclaredType>(string name)
        where TDeclaredType : ASType
    {
        // Lookup the entry
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

    public static ASTypeRegistry Create()
    {
        var typeMap = new Dictionary<string, TypeRegistryEntry>();

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
                var typeAttributes = type.GetCustomAttributes<ASTypeKeyAttribute>();
                foreach (var asObjectType in typeAttributes)
                {
                    // Have to lowercase this for accurate checking 
                    var typeName = asObjectType.Type.ToLower();
                    
                    // Check for dupes
                    if (typeMap.TryGetValue(typeName, out var originalType))
                        throw new ApplicationException($"Multiple classes are using AS type name {typeName}: trying to register {type} on top of {originalType}");

                    // Create an appropriate entry for the type
                    var entry = type.IsOpenGeneric()
                        ? new GenericTypeRegistryEntry(type, asObjectType.DefaultGenerics)
                        : new TypeRegistryEntry(type);
                    
                    // Done - cache it
                    typeMap[typeName] = entry;
                }
            }
        }

        return new ASTypeRegistry(typeMap);
    }

    private class TypeRegistryEntry
    {
        internal Type RegisteredType { get; }

        public TypeRegistryEntry(Type registeredType) => RegisteredType = registeredType;

        internal virtual Type ReifyType<TDeclaredType>() where TDeclaredType : ASType => RegisteredType;
    }

    private class GenericTypeRegistryEntry : TypeRegistryEntry
    {
        private Type[]? DefaultGenerics { get; }
        private readonly Dictionary<Type, Type> _reifiedCache = new();

        public GenericTypeRegistryEntry(Type registeredType, Type[]? defaultGenerics) : base(registeredType)
        {
            if (!registeredType.IsOpenGeneric())
                throw new ArgumentException($"Type {registeredType} is not an open generic", nameof(registeredType));
            
            DefaultGenerics = defaultGenerics;
        }

        internal override Type ReifyType<TDeclaredType>()
        {
            var declaredType = typeof(TDeclaredType);
            
            // Fast path: check for cache
            if (_reifiedCache.TryGetValue(declaredType, out var reifiedType))
                return reifiedType;

            // Populate a list of concrete types to fill generic slots
            var declaredSlots = declaredType.TryGetGenericArgumentsFor(RegisteredType);
            var genericSlots = RegisteredType.GetGenericArguments();
            for (var i = 0; i < genericSlots.Length; i++)
            {
                var slot = genericSlots[i];

                // First try to get from declared type
                if (slot.IsOpenGeneric() && declaredSlots != null && declaredSlots.Length >= genericSlots.Length)
                    slot = declaredSlots[i];

                // Then try to get from registered defaults
                if (slot.IsOpenGeneric() && DefaultGenerics != null && DefaultGenerics.Length >= genericSlots.Length)
                    slot = DefaultGenerics[i];

                // Finally fall back to global default
                if (slot.IsOpenGeneric())
                    slot = typeof(ASType);

                genericSlots[i] = slot;
            }
            
            // Construct the type and cache it for performance
            reifiedType = RegisteredType.MakeGenericType(genericSlots);
            _reifiedCache[declaredType] = reifiedType;
            
            return reifiedType;
        }
    }
}