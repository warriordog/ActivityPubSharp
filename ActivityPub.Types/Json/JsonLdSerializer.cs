// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Internal.TypeInfo;

namespace ActivityPub.Types.Json;

/// <summary>
/// Provides support for (de)serializing JSON-LD to/from .NET objects.
/// </summary>
public interface IJsonLdSerializer
{
    /// <summary>
    /// Options in use by this instance.
    /// Will become read-only after first use.
    /// </summary>
    JsonSerializerOptions SerializerOptions { get; }

    /// <inheritdoc cref="JsonSerializer.Deserialize{T}(string, JsonSerializerOptions)"/>
    public T? Deserialize<T>(string json);

    /// <inheritdoc cref="JsonSerializer.Deserialize(string, Type, JsonSerializerOptions)"/>
    public object? Deserialize(string json, Type type);

    /// <inheritdoc cref="JsonSerializer.Serialize{T}(T, JsonSerializerOptions)"/>
    public string Serialize<T>(T? value);

    /// <inheritdoc cref="JsonSerializer.SerializeToElement{T}(T, JsonSerializerOptions)"/>
    public JsonElement SerializeToElement<T>(T? value);
}

public class JsonLdSerializer : IJsonLdSerializer
{
    public JsonSerializerOptions SerializerOptions { get; }

    public JsonLdSerializer(IASTypeInfoCache asTypeInfoCache, IJsonTypeInfoCache jsonTypeInfoCache)
    {
        SerializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        SerializerOptions.Converters.Add(new ASTypeConverter(asTypeInfoCache, jsonTypeInfoCache));
        SerializerOptions.Converters.Add(new LinkableConverter(asTypeInfoCache));
        SerializerOptions.Converters.Add(new ListableConverter());
    }

    public T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, SerializerOptions);
    public object? Deserialize(string json, Type type) => JsonSerializer.Deserialize(json, type, SerializerOptions);

    public string Serialize<T>(T? value) => JsonSerializer.Serialize(value, SerializerOptions);
    public JsonElement SerializeToElement<T>(T? value) => JsonSerializer.SerializeToElement(value, SerializerOptions);
}