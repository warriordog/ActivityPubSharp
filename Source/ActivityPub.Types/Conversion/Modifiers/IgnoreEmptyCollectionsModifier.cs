using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;
using ActivityPub.Types.Internal;

namespace ActivityPub.Types.Conversion.Modifiers;

/// <summary>
///     Contract modifier that configures <a href="https://www.nuget.org/packages/System.Text.Json/">System.Text.Json</a> to ignore empty collections from the JSON output.
/// </summary>
public static class IgnoreEmptyCollectionsModifier
{
    /// <summary>
    ///     Adds <see cref="IgnoreEmptyCollections"/> to the provided modifier
    /// </summary>
    public static T WithIgnoreEmptyCollections<T>(this T resolver)
        where T : DefaultJsonTypeInfoResolver
    {
        resolver.Modifiers.Add(IgnoreEmptyCollections);
        return resolver;
    }

    /// <summary>
    ///     On serialization, dynamically ignores any properties that are a collection with zero elements.
    /// </summary>
    public static void IgnoreEmptyCollections(JsonTypeInfo jsonTypeInfo)
    {
        // Only applies to objects
        if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        // Modify all collection properties
        foreach (var prop in jsonTypeInfo.Properties)
        {
            // Non-generic ICollection is the easy case
            if (prop.PropertyType.IsAssignableTo(typeof(ICollection)))
                BindCollection(prop);

            // Generic ICollection also counts
            else if (prop.PropertyType.TryGetGenericArgumentsFor(typeof(ICollection<>), out var genericSlots))
                BindGenericCollection(prop, genericSlots[0]);

            // Anything deriving from IReadOnlyCollection is in scope, since this is serialization only
            else if (prop.PropertyType.TryGetGenericArgumentsFor(typeof(IReadOnlyCollection<>), out var readOnlySlots))
                // Add a callback to conditionally exclude the prop
                BindReadOnlyCollection(prop, readOnlySlots[0]);
        }
    }

    private static void BindCollection(JsonPropertyInfo prop)
    {
        var wrapper = new ShouldSerializeWrapper<ICollection>
        {
            Fallback = prop.ShouldSerialize,
            Condition = (_, collection) => collection.Count > 0
        };
        prop.ShouldSerialize = wrapper.ShouldSerialize;
    }

    private static void BindGenericCollection(JsonPropertyInfo prop, Type itemType)
    {
        var method =
                typeof(IgnoreEmptyCollectionsModifier)
                    .GetMethod(nameof(BindGenericCollectionOf), BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.NonPublic)
                ?? throw new ApplicationException("bug: missing required method BindGenericCollectionOf")
            ;

        method
            .MakeGenericMethod(itemType, prop.PropertyType)
            .Invoke(null, [prop])
            ;
    }

    private static void BindGenericCollectionOf<TItem, TCollection>(JsonPropertyInfo prop)
        where TCollection : ICollection<TItem>
    {
        var wrapper = new ShouldSerializeWrapper<TCollection>
        {
            Fallback = prop.ShouldSerialize,
            Condition = (_, collection) => collection.Count == 0
        };
        prop.ShouldSerialize = wrapper.ShouldSerialize;
    }

    private static void BindReadOnlyCollection(JsonPropertyInfo prop, Type itemType)
    {
        var method =
            typeof(IgnoreEmptyCollectionsModifier)
                .GetMethod(nameof(BindReadOnlyCollectionOf), BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.NonPublic)
            ?? throw new ApplicationException("bug: missing required method BindReadOnlyCollectionOf");

        method
            .MakeGenericMethod(itemType, prop.PropertyType)
            .Invoke(null, [prop]);
    }

    private static void BindReadOnlyCollectionOf<TItem, TCollection>(JsonPropertyInfo prop)
        where TCollection : IReadOnlyCollection<TItem>
    {
        var wrapper = new ShouldSerializeWrapper<TCollection>
        {
            Fallback = prop.ShouldSerialize,
            Condition = (_, collection) => collection.Count == 0
        };
        prop.ShouldSerialize = wrapper.ShouldSerialize;
    }

    private class ShouldSerializeWrapper<T>
    {
        public required Func<object, object?, bool>? Fallback { get; init; }
        public required Func<object, T, bool?> Condition { get; init; }

        public bool ShouldSerialize(object obj, object? value) =>
            InvokeCondition(obj, value)
            ?? Fallback?.Invoke(obj, value)
            ?? false;

        private bool? InvokeCondition(object obj, object? value)
        {
            if (value is not T tValue)
                return null;

            return Condition(obj, tValue);
        }
    }
}