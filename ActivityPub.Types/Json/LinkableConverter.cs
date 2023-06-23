/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */


using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Json;

public class LinkableConverter : JsonConverterFactory
{
    // We only convert Linkable<T>
    public override bool CanConvert(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Linkable<>);

    // Pivot the type into correct instance
    public override JsonConverter? CreateConverter(Type type, JsonSerializerOptions options)
    {
        var valueType = type.GetGenericArguments()[0];
        return (JsonConverter)Activator.CreateInstance(
            typeof(LinkableConverter<>).MakeGenericType(valueType),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: new object[] { options },
            culture: null
        )!;
    }
}

public class LinkableConverter<T> : JsonConverter<Linkable<T>>
{
    public LinkableConverter(JsonSerializerOptions options)
    {
        // TODO cache stuff
    }
    
    public override Linkable<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;

        // Objects that aren't ASLink are the payload data
        if (!IsASLink(reader))
        {
            var obj = JsonSerializer.Deserialize<T>(ref reader, options);
            return (Linkable<T>?)Activator.CreateInstance(typeToConvert, obj);
        }

        // Delegate link construction back to the parser
        var link = JsonSerializer.Deserialize<ASLink>(ref reader, options);
        return (Linkable<T>?)Activator.CreateInstance(typeToConvert, link);
    }

    public override void Write(Utf8JsonWriter writer, Linkable<T> linkable, JsonSerializerOptions options)
    {
        if (linkable.TryGetLink(out var link))
        {
            writer.WriteStringValue(link.HRef);
        }
        else if (linkable.TryGetValue(out var value))
        {
            JsonSerializer.Serialize(writer, value);
        }
        else
        {
            throw new ArgumentException("Linkable<T> is invalid - it has neither a link nor a value");
        }
    }

    // Intentionally NOT passed by ref, to ensure we get a copy
    private static bool IsASLink(Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            return false;
        
        while (reader.Read())
        {
            // Empty or unknown object does NOT count as link
            if (reader.TokenType == JsonTokenType.EndObject)
                return false;
            
            // Skip any nested objects
            if (reader.TokenType == JsonTokenType.StartObject)
                while (reader.TokenType != JsonTokenType.EndObject)
                    reader.Read();

            // Skip any nested arrays
            else if (reader.TokenType == JsonTokenType.StartArray)
                while (reader.TokenType != JsonTokenType.EndArray)
                    reader.Read();

            // Read the "type" property
            else if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "type")
            {
                reader.Read();

                // Directly compare strings
                if (reader.TokenType == JsonTokenType.String)
                    return reader.GetString() == ASLink.LinkType;

                // Search within arrays
                if (reader.TokenType == JsonTokenType.StartArray)
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                        if (reader.TokenType == JsonTokenType.String && reader.GetString() == ASLink.LinkType)
                            return true;

                // If we get here, then type is invalid and this isn't a link.
                return false;
            }
        }

        return false;
    }
}