/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ActivityPub.Types.Json;


public class LinkableConverter : JsonConverter<ILinkable>
{
    public override void WriteJson(JsonWriter writer, ILinkable? value, JsonSerializer serializer)
    {
        var jsonValue = value?.GetValue();
        serializer.Serialize(writer, jsonValue);
    }
    
    public override ILinkable? ReadJson(JsonReader reader, Type objectType, ILinkable? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        
        if (token.Type == JTokenType.Null)
        {
            return null;
        }

        if (token.Type == JTokenType.String)
        {
            var str = token.ToObject<string>(serializer) ?? throw new JsonException("string turned into null - should not happen");
            var link = new ASLink { HRef = str };
            return (ILinkable?)Activator.CreateInstance(objectType, link);
        }

        if (token.Type == JTokenType.Object)
        {
            var obj = token.ToObject<ASType>(serializer);
            return (ILinkable?)Activator.CreateInstance(objectType, obj);
        }

        throw new JsonException($"Invalid JToken type for Linkable - {token.Type}");
    }
}