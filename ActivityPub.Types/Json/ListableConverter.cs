/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Json;

public class ListableConverter : JsonConverterFactory
{
    // We only convert concrete types deriving from ICollection<T>
    public override bool CanConvert(Type type) => type.IsAssignableToGenericType(typeof(ICollection<>));

    // Pivot the type into correct converter
    public override JsonConverter? CreateConverter(Type collectionType, JsonSerializerOptions options)
    {
        var itemType = collectionType.GetGenericArgumentsFor(typeof(ICollection<>))[0];
        
        return (JsonConverter)Activator.CreateInstance(
            typeof(ListableConverter<,>).MakeGenericType(itemType, collectionType),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: new object[] { options },
            culture: null
        )!;
    }
}

public class ListableConverter<TItem, TCollection> : JsonConverter<TCollection>
where TItem : class
where TCollection : class, ICollection<TItem>, new()
{
    public ListableConverter(JsonSerializerOptions options)
    {
        // TODO cache stuff
    }
    
    public override TCollection? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return JsonSerializer.Deserialize<TCollection>(ref reader, options);
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            var item = JsonSerializer.Deserialize<TItem>(ref reader, options) ?? throw new JsonException("Failed to deserialize object for Listable");
            return new TCollection { item };
        }
        
        throw new JsonException($"Cannot convert {reader.TokenType} into Listable<T>");
    }

    public override void Write(Utf8JsonWriter writer, TCollection collection, JsonSerializerOptions options)
    {
        // If value is a single-element collection, then unpack it
        if (collection?.Count == 1)
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