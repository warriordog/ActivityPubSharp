/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Internal;

namespace ActivityPub.Types.Json;

public class ListableConverter : JsonConverterFactory
{
    // We only convert concrete types deriving from ICollection<T>
    public override bool CanConvert(Type type) => type.IsAssignableToGenericType(typeof(ICollection<>));

    // Pivot the type into correct converter
    public override JsonConverter? CreateConverter(Type collectionType, JsonSerializerOptions options)
    {
        var itemType = collectionType.GetGenericArgumentsFor(typeof(ICollection<>))[0];
        var converterType = typeof(ListableConverter<,>).MakeGenericType(itemType, collectionType);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

internal class ListableConverter<TItem, TCollection> : JsonConverter<TCollection>
    where TItem : class
    where TCollection : class, ICollection<TItem>, new()
{
    public override TCollection? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;

        // If array, then deserialize directly to the collection
        if (reader.TokenType == JsonTokenType.StartArray)
            return JsonSerializer.Deserialize<TCollection>(ref reader, options);

        // Otherwise we assume (hope) that the next token can be deserialized to TItem
        var item = JsonSerializer.Deserialize<TItem>(ref reader, options) ??
                   throw new JsonException("Failed to deserialize object for Listable");
        return new TCollection { item };
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
            JsonSerializer.Serialize(writer, collection, options);
        }
    }
}