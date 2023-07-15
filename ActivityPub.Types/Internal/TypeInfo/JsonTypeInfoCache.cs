// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using ActivityPub.Types.Json;
using InternalUtils;

namespace ActivityPub.Types.Internal.TypeInfo;

/// <summary>
/// Caches JSON serialization metadata for types within the application.
/// Used by <see cref="ASTypeConverter"/> to work around System.Text.Json bugs 
/// </summary>
public interface IJsonTypeInfoCache
{
    /// <summary>
    /// Gets type info for a particular type.
    /// If not in the cache, then info will be created.
    /// </summary>
    /// <param name="type">Type to lookup</param>
    public JsonTypeInfo GetForType(Type type);

    /// <summary>
    /// Gets type info for a particular type.
    /// If not in the cache, then info will be created.
    /// </summary>
    /// <typeparam name="T">Type to lookup</typeparam>
    public JsonTypeInfo GetForType<T>();
}

public class JsonTypeInfoCache : IJsonTypeInfoCache
{
    private readonly Dictionary<Type, JsonTypeInfo> _typeInfoMap = new();

    public JsonTypeInfo GetForType(Type type)
    {
        if (type.IsOpenGeneric())
            throw new ArgumentException($"Cannot create JsonTypeInfo for an open generic type {type}", nameof(type));

        if (!_typeInfoMap.TryGetValue(type, out var info))
        {
            info = CreateForType(type);
            _typeInfoMap[type] = info;
        }

        return info;
    }
    public JsonTypeInfo GetForType<T>()
    {
        var type = typeof(T);
        if (!_typeInfoMap.TryGetValue(type, out var info))
        {
            info = CreateForType<T>();
            _typeInfoMap[type] = info;
        }

        return info;
    }


    // Ugly hack to pivot from Method(Type) to Method<Type>()
    private static readonly MethodInfo CreateForTypeMethod =
        typeof(JsonTypeInfoCache)
            .GetMethod(nameof(CreateForType), BindingFlags.NonPublic | BindingFlags.Static, Type.EmptyTypes)
        ?? throw new ApplicationException($"Runtime error - can't find {nameof(JsonTypeInfoCache)}.{nameof(CreateForType)}()");

    // Pivot to the generic overload.
    // Its an inefficient hack, but this is only called *at most* once per type.  
    private static JsonTypeInfo CreateForType(Type type)
    {
        var method = CreateForTypeMethod.MakeGenericMethod(type);
        return (JsonTypeInfo)method.Invoke(null, null)!;
    }

    private static JsonTypeInfo<T> CreateForType<T>()
    {
        var type = typeof(T);

        FindJsonProperties(type, out var getters, out var setters);
        FindCustomSerializationMethods<T>(type, out var trySerialize, out var tryDeserialize);

        return new JsonTypeInfo<T>
        {
            Type = type,
            Setters = setters.ToArray(),
            Getters = getters.ToArray(),
            CustomSerializer = trySerialize,
            CustomDeserializer = tryDeserialize
        };
    }


    private static void FindJsonProperties(Type type, out JsonPropertyInfo[] getters, out JsonPropertyInfo[] setters)
    {
        var getterList = new List<JsonPropertyInfo>();
        var setterList = new List<JsonPropertyInfo>();

        //https://stackoverflow.com/a/12330276
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
        {
            // Skip if its ignored
            if (property.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                continue;

            // Get the name
            var propertyNameAttr = property.GetCustomAttribute<JsonPropertyNameAttribute>();
            var name = propertyNameAttr?.Name ?? property.Name;

            // Create the info obj
            var propInfo = new JsonPropertyInfo
            {
                Property = property,
                Name = name,

                // https://stackoverflow.com/questions/74371619/c-sharp-11-detect-required-property-by-reflection
                IsRequired = Attribute.IsDefined(property, typeof(RequiredMemberAttribute))
            };

            // Add to appropriate lists
            if (property.GetMethod != null)
                getterList.Add(propInfo);
            if (property.SetMethod != null)
                setterList.Add(propInfo);
        }

        // Pack into arrays for efficiency.
        // We're never going to modify these again
        getters = getterList.ToArray();
        setters = setterList.ToArray();
    }

    // This is weird because we're trying to find both methods (which are optional) and check for duplicates all in one pass.
    private static void FindCustomSerializationMethods<T>(Type type, out TrySerializeDelegate<T>? trySerialize, out TryDeserializeDelegate<T>? tryDeserialize)
    {
        trySerialize = null;
        tryDeserialize = null;

        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly))
        {
            // Check if its the serializer
            var serializeAttr = method.GetCustomAttribute<CustomJsonSerializerAttribute>();
            if (serializeAttr != null)
            {
                if (trySerialize != null)
                    throw new ApplicationException($"Malformed AS entity {type}: only one method can be annotated with {nameof(CustomJsonSerializerAttribute)}");

                trySerialize = method.CreateDelegate<TrySerializeDelegate<T>>();
            }

            // Check if its the deserializer
            var deserializeAttr = method.GetCustomAttribute<CustomJsonDeserializerAttribute>();
            if (deserializeAttr != null)
            {
                if (tryDeserialize != null)
                    throw new ApplicationException($"Malformed AS entity {type}: only one method can be annotated with {nameof(CustomJsonDeserializerAttribute)}");

                tryDeserialize = method.CreateDelegate<TryDeserializeDelegate<T>>();
            }
        }
    }
}