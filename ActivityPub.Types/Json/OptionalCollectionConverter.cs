// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Json;

public class OptionalCollectionConverter : JsonConverterFactory
{
    // We only convert concrete types deriving from ICollection<T>
    public override bool CanConvert(Type type) => type.IsAssignableToGenericType(typeof(ICollection<>));

    // Pivot the type into correct converter
    public override JsonConverter CreateConverter(Type collectionType, JsonSerializerOptions options)
    {
        var itemType = collectionType.GetGenericArgumentsFor(typeof(ICollection<>))[0];

        return (JsonConverter)Activator.CreateInstance(
            typeof(OptionalCollectionConverter<,>).MakeGenericType(itemType, collectionType),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: new object[] { options },
            culture: null
        )!;
    }
}

public class OptionalCollectionConverter<TItem, TCollection> : JsonConverter<TCollection>
    where TCollection : ICollection<TItem>
{
    public OptionalCollectionConverter(JsonSerializerOptions options)
    {
        // TODO cache stuff
    }

    // Read is a no-op
    public override TCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        JsonSerializer.Deserialize<TCollection>(ref reader, options);

    public override void Write(Utf8JsonWriter writer, TCollection collection, JsonSerializerOptions options)
    {
        if (collection.Count == 0)
        {
            writer.WriteNullValue();
        }
        else
        {
            JsonSerializer.Serialize(writer, collection, options);
        }
    }
}