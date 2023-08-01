// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using ActivityPub.Types.Json;
using ActivityPub.Types.Json.Attributes;
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
            .GetMethod(nameof(CreateForType), BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly, Type.EmptyTypes)
        ?? throw new ApplicationException($"Runtime error - can't find {nameof(JsonTypeInfoCache)}.{nameof(CreateForType)}()");

    private static readonly MethodInfo CreatePropertyInfoOfMethod =
        typeof(JsonTypeInfoCache)
            .GetMethod(nameof(CreatePropertyInfoOf), BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)
        ?? throw new MissingMemberException(nameof(JsonTypeInfoCache), nameof(CreatePropertyInfoOf));

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

        FindJsonProperties(type, out var getters, out var requiredSetters, out var optionalSetters);

        return new JsonTypeInfo<T>
        {
            Type = type,
            RequiredSetters = requiredSetters,
            OptionalSetters = optionalSetters,
            Getters = getters.ToArray(),
            CustomSerializer = FindCustomSerializer(type, type)?.CreateDelegate<TrySerializeDelegate<T>>(),
            CustomDeserializer = FindCustomDeserializer(type, type)?.CreateDelegate<TryDeserializeDelegate<T>>()
        };
    }

    private static void FindJsonProperties(Type type, out JsonPropertyInfo[] getters, out Dictionary<string, JsonPropertyInfo> requiredSetters, out Dictionary<string, JsonPropertyInfo> optionalSetters)
    {
        requiredSetters = new();
        optionalSetters = new();
        var getterList = new List<JsonPropertyInfo>();

        //https://stackoverflow.com/a/12330276
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
        {
            // Skip if its ignored
            var jsonIgnoreAttr = property.GetCustomAttribute<JsonIgnoreAttribute>();
            if (jsonIgnoreAttr is { Condition: JsonIgnoreCondition.Always })
                continue;

            // Get the name
            var propertyNameAttr = property.GetCustomAttribute<JsonPropertyNameAttribute>();
            var name = propertyNameAttr?.Name ?? property.Name;

            // https://stackoverflow.com/questions/74371619/c-sharp-11-detect-required-property-by-reflection
            var isRequired = Attribute.IsDefined(property, typeof(RequiredMemberAttribute));

            // Create the info obj
            var propInfo = CreatePropertyInfo(
                // Basic property details
                name,
                isRequired,

                // Metadata needed for serialization
                property,
                property.PropertyType.GetDefaultValue(),
                property.PropertyType.IsAssignableTo(typeof(ICollection)),
                jsonIgnoreAttr?.Condition,
                Attribute.IsDefined(property, typeof(JsonIgnoreWhenNestedAttribute)),

                // Custom converters
                property.GetCustomAttribute<JsonConverterAttribute>()?.ConverterType,
                FindCustomSerializer(type, property),
                FindCustomDeserializer(type, property)
            );

            // Add to appropriate lookups
            if (property.GetMethod != null)
            {
                getterList.Add(propInfo);
            }

            if (property.SetMethod != null)
            {
                if (isRequired)
                    requiredSetters[name] = propInfo;
                optionalSetters[name] = propInfo;
            }
        }

        // Pack into arrays for efficiency.
        // We're never going to modify these again
        getters = getterList.ToArray();
    }

    private static JsonPropertyInfo CreatePropertyInfo(string name, bool isRequired, PropertyInfo property, object? typeDefaultValue, bool isCollection, JsonIgnoreCondition? ignoreCondition, bool ignoreWhenNested, Type? customConverterType, MethodInfo? customSerializerMethod, MethodInfo? customDeserializerMethod)
    {
        // Pivot to generic method
        var method = CreatePropertyInfoOfMethod.MakeGenericMethod(property.PropertyType);
        return (JsonPropertyInfo)method.Invoke(null, new[] { name, isRequired, property, typeDefaultValue, isCollection, ignoreCondition, ignoreWhenNested, customConverterType, customSerializerMethod, customDeserializerMethod })!;
    }

    private static JsonPropertyInfo<T> CreatePropertyInfoOf<T>(string name, bool isRequired, PropertyInfo property, object? typeDefaultValue, bool isCollection, JsonIgnoreCondition? ignoreCondition, bool ignoreWhenNested, Type? customConverterType, MethodInfo? customSerializerMethod, MethodInfo? customDeserializerMethod)
    {
        var customSerializer = customSerializerMethod?.CreateDelegate<TrySerializeDelegate<T>>();
        var customDeserializer = customDeserializerMethod?.CreateDelegate<TryDeserializeDelegate<T>>();

        return new JsonPropertyInfo<T>
        {
            // Basic property details
            Name = name,
            IsRequired = isRequired,

            // Metadata needed for serialization
            Property = property,
            TypeDefaultValue = typeDefaultValue,
            IsCollection = isCollection,
            IgnoreCondition = ignoreCondition,
            IgnoreWhenNested = ignoreWhenNested,
            CustomConverterType = customConverterType,

            // Customer converters
            CustomDeserializer = customDeserializer,
            CustomSerializer = customSerializer
        };
    }

    private static MethodInfo? FindCustomSerializer(Type type, MemberInfo member)
    {
        var customSerializerAttribute = member.GetCustomAttribute<CustomJsonSerializerAttribute>();
        if (customSerializerAttribute?.MethodName == null)
            return null;

        var customSerializer = type.GetMethod(customSerializerAttribute.MethodName);
        if (customSerializer == null)
            throw new MissingMethodException(type.Name, customSerializerAttribute.MethodName);

        return customSerializer;
    }

    private static MethodInfo? FindCustomDeserializer(Type type, MemberInfo member)
    {
        var customDeserializerAttribute = member.GetCustomAttribute<CustomJsonDeserializerAttribute>();
        if (customDeserializerAttribute?.MethodName == null)
            return null;

        var customDeserializer = type.GetMethod(customDeserializerAttribute.MethodName);
        if (customDeserializer == null)
            throw new MissingMethodException(type.Name, customDeserializerAttribute.MethodName);

        return customDeserializer;
    }
}