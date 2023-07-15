// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using InternalUtils;

namespace ActivityPub.Types.Json;

/// <summary>
/// Converts types that can be a list of elements, or a single elements.
/// Essentially: T | T[]
/// </summary>
public class ListableConverter : JsonConverterFactory
{
    public override bool CanConvert(Type type) =>
        // We only convert concrete types deriving from ICollection<T>
        type.IsAssignableToGenericType(typeof(ICollection<>))

        // This has to be registered globally, which causes it to pick up dictionaries.
        // The code below ends up with a null key and goes BOOM, which is less than idea.
        // Its kind of a hack, but we avoid the issue by ignoring all dictionaries.
        && !type.IsAssignableToGenericType(typeof(IDictionary<,>));

    // Pivot the type into correct converter
    public override JsonConverter? CreateConverter(Type collectionType, JsonSerializerOptions options)
    {
        var itemType = collectionType.GetGenericArgumentsFor(typeof(ICollection<>))[0];
        var converterType = typeof(ListableConverter<,>).MakeGenericType(itemType, collectionType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

internal class ListableConverter<TItem, TCollection> : JsonConverter<TCollection>
    where TCollection : class, ICollection<TItem>, new()
{
    public override TCollection? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;

            // If array, then deserialize directly to the collection
            case JsonTokenType.StartArray:
            {
                var collection = new TCollection();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    var item = ReadItem(ref reader, options);
                    collection.Add(item);
                }

                return collection;
            }

            // Otherwise we assume (hope) that the next token can be deserialized to TItem
            default:
            {
                var item = ReadItem(ref reader, options);
                return new TCollection { item };
            }
        }
    }

    private static TItem ReadItem(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var item = JsonSerializer.Deserialize<TItem>(ref reader, options);

        if (item == null)
            throw new JsonException($"Failed to deserialize {typeof(TItem)} for {typeof(TCollection)}");

        return item;
    }


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
            {
                JsonSerializer.Serialize(writer, item, options);
            }

            writer.WriteEndArray();
        }
    }
}