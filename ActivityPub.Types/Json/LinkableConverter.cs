/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */


using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Json;

public class LinkableConverter : JsonConverterFactory
{
    // We only convert Linkable<T>
    public override bool CanConvert(Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Linkable<>);

    // Pivot the type into correct instance
    public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
    {
        var valueType = type.GetGenericArguments()[0];
        var converterType = typeof(LinkableConverter<>).MakeGenericType(valueType);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

internal class LinkableConverter<T> : JsonConverter<Linkable<T>>
{
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
            JsonSerializer.Serialize(writer, value, options);
        }
        else
        {
            throw new ArgumentException("Linkable<T> is invalid - it has neither a link nor a value");
        }
    }

    // Intentionally NOT passed by ref, to ensure we get a copy
    private static bool IsASLink(Utf8JsonReader reader) => reader.TryGetASObjectType(out var type) && type == ASLink.LinkType;
}