/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ActivityPub.Types.Json;

public class ListableConverter<T> : JsonConverter<ICollection<T>>
where T : class
{
    public override void WriteJson(JsonWriter writer, ICollection<T>? collection, JsonSerializer serializer)
    {
        // If value is a single-element collection, then unpack it
        if (collection?.Count == 1)
        {
            var flatValue = collection.First();
            serializer.Serialize(writer, flatValue);
        }
        else
        {
            serializer.Serialize(writer, collection);
        }
    }

    // RIP null and type safety
    public override ICollection<T>? ReadJson(JsonReader reader, Type objectType, ICollection<T>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        // Get or create list to contain results
        ICollection<T> collection;
        if (hasExistingValue)
        {
            collection = existingValue!;
        }
        else
        {
            // YOLO
            collection = (ICollection<T>)Activator.CreateInstance(objectType)!;
        }
        
        var token = JToken.Load(reader);
        if (token.Type == JTokenType.Array)
        {
            foreach (var child in token.Children())
            {
                var item = child.ToObject<T>(serializer);
                collection.Add(item!);
            }
        }
        else
        {
            var item = token.ToObject<T>(serializer);
            collection.Add(item!);
        }

        return collection;
    }
}