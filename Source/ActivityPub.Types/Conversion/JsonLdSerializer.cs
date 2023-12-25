// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using ActivityPub.Types.Conversion.Converters;
using ActivityPub.Types.Conversion.Modifiers;
using Microsoft.Extensions.Options;

namespace ActivityPub.Types.Conversion;

/// <summary>
///     Provides support for (de)serializing JSON-LD to/from .NET objects.
/// </summary>
public interface IJsonLdSerializer
{
    /// <summary>
    ///     Options in use by this instance.
    ///     Will become read-only after first use.
    /// </summary>
    JsonSerializerOptions SerializerOptions { get; }

    /// <inheritdoc cref="JsonSerializer.Deserialize{T}(string, JsonSerializerOptions)" />
    public T? Deserialize<T>(string json);
    
    /// <inheritdoc cref="JsonSerializer.Deserialize(string, Type, JsonSerializerOptions)" />
    public object? Deserialize(string json, Type type);
    
    
    /// <inheritdoc cref="JsonSerializer.Deserialize{T}(Stream, JsonSerializerOptions)" />
    public T? Deserialize<T>(Stream json);

    /// <inheritdoc cref="JsonSerializer.Deserialize(Stream, Type, JsonSerializerOptions)" />
    public object? Deserialize(Stream json, Type type);
    

    /// <inheritdoc cref="JsonSerializer.DeserializeAsync{T}(Stream, JsonSerializerOptions, CancellationToken)" />
    public ValueTask<T?> DeserializeAsync<T>(Stream json);
    
    /// <inheritdoc cref="JsonSerializer.DeserializeAsync(Stream, Type, JsonSerializerOptions, CancellationToken)" />
    public ValueTask<object?> DeserializeAsync(Stream json, Type type);

    
    /// <inheritdoc cref="JsonSerializer.Serialize{T}(T, JsonSerializerOptions)" />
    public string Serialize<T>(T? value);

    
    /// <inheritdoc cref="JsonSerializer.SerializeToElement{T}(T, JsonSerializerOptions)" />
    public JsonElement SerializeToElement<T>(T? value);
}

/// <summary>
///     Default implementation of <see cref="IJsonLdSerializer"/>.
/// </summary>
public class JsonLdSerializer : IJsonLdSerializer
{
    /// <summary>
    ///     Constructs a new instance of <see cref="JsonLdSerializer"/> from the provided services.
    ///     This is meant to be called implicitly by a DI container, but can be used directly for testing or advanced use cases.
    /// </summary>
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameterInConstructor")]
    public JsonLdSerializer
    (
        IOptions<JsonLdSerializerOptions> serializerOptions,
        TypeMapConverter typeMapConverter,
        ASTypeConverter asTypeConverter,
        LinkableConverter linkableConverter,
        ListableConverter listableConverter,
        ListableReadOnlyConverter listableReadOnlyConverter
    )
        => SerializerOptions = new JsonSerializerOptions(serializerOptions.Value.DefaultJsonSerializerOptions)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                .WithIgnoreEmptyCollections()
                .WithBugFixes(),
            Converters =
            {
                typeMapConverter,
                asTypeConverter,
                linkableConverter,
                listableConverter,
                listableReadOnlyConverter
            }
        };

    /// <inheritdoc />
    public JsonSerializerOptions SerializerOptions { get; }

    /// <inheritdoc />
    public T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, SerializerOptions);

    /// <inheritdoc />
    public object? Deserialize(string json, Type type) => JsonSerializer.Deserialize(json, type, SerializerOptions);
    
    
    /// <inheritdoc />
    public T? Deserialize<T>(Stream json) => JsonSerializer.Deserialize<T>(json, SerializerOptions);
    
    /// <inheritdoc />
    public object? Deserialize(Stream json, Type type) => JsonSerializer.Deserialize(json, type, SerializerOptions);
    
    
    /// <inheritdoc />
    public ValueTask<T?> DeserializeAsync<T>(Stream json) => JsonSerializer.DeserializeAsync<T>(json, SerializerOptions);
    
    /// <inheritdoc />
    public ValueTask<object?> DeserializeAsync(Stream json, Type type) => JsonSerializer.DeserializeAsync(json, type, SerializerOptions);

    
    /// <inheritdoc />
    public string Serialize<T>(T? value) => JsonSerializer.Serialize(value, SerializerOptions);

    
    /// <inheritdoc />
    public JsonElement SerializeToElement<T>(T? value) => JsonSerializer.SerializeToElement(value, SerializerOptions);
}