// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using InternalUtils;

namespace ActivityPub.Types.Conversion.Converters;

/// <summary>
///     Converts types that can be a list of elements, or a single elements.
///     Essentially: T | T[]
/// </summary>
internal class ListableReadOnlyConverter : JsonConverterFactory
{
    public override bool CanConvert(Type type) =>
        // We only convert concrete types deriving from ICollection<T>
        type.IsAssignableToGenericType(typeof(IReadOnlyCollection<>))

        // This has to be registered globally, which causes it to pick up dictionaries.
        // The code below ends up with a null key and goes BOOM, which is less than idea.
        // Its kind of a hack, but we avoid the issue by ignoring all dictionaries.
        && !type.IsAssignableToGenericType(typeof(IReadOnlyDictionary<,>));

    // Pivot the type into correct converter
    public override JsonConverter? CreateConverter(Type collectionType, JsonSerializerOptions options)
    {
        var itemType = collectionType.GetGenericArgumentsFor(typeof(IReadOnlyCollection<>))[0];
        var converterType = typeof(ListableReadOnlyConverter<,>).MakeGenericType(itemType, collectionType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

internal class ListableReadOnlyConverter<TItem, TCollection> : JsonConverter<TCollection>
    where TCollection : IReadOnlyCollection<TItem>
{
    public override TCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new InvalidOperationException($"Cannot write to read-only collection of type {typeof(TCollection)}");

    public override void Write(Utf8JsonWriter writer, TCollection collection, JsonSerializerOptions options)
    {
        // If value is a single-element collection, then unpack it
        if (collection.Count == 1)
        {
            var item = collection.First();
            JsonSerializer.Serialize(writer, item, options);
        }
        else
        {
            writer.WriteStartArray();
            foreach (var item in collection)
                JsonSerializer.Serialize(writer, item, options);

            writer.WriteEndArray();
        }
    }
}