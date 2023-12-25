// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
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
    
    
    /// <inheritdoc cref="JsonSerializer.Deserialize{T}(ReadOnlySpan{char}, JsonSerializerOptions)" />
    public T? Deserialize<T>(ReadOnlySpan<char> json);
    
    /// <inheritdoc cref="JsonSerializer.Deserialize(ReadOnlySpan{char}, Type, JsonSerializerOptions)" />
    public object? Deserialize(ReadOnlySpan<char> json, Type type);
    
    
    /// <inheritdoc cref="JsonSerializer.Deserialize{T}(ReadOnlySpan{byte}, JsonSerializerOptions)" />
    public T? Deserialize<T>(ReadOnlySpan<byte> json);
    
    /// <inheritdoc cref="JsonSerializer.Deserialize(ReadOnlySpan{byte}, Type, JsonSerializerOptions)" />
    public object? Deserialize(ReadOnlySpan<byte> json, Type type);
    
    
    /// <inheritdoc cref="JsonSerializer.Deserialize{T}(ref Utf8JsonReader, JsonSerializerOptions)" />
    public T? Deserialize<T>(ref Utf8JsonReader reader);

    /// <inheritdoc cref="JsonSerializer.Deserialize(ref Utf8JsonReader, Type, JsonSerializerOptions)" />
    public object? Deserialize(ref Utf8JsonReader reader, Type type);
    
    
    /// <inheritdoc cref="JsonSerializer.Deserialize{T}(JsonElement, JsonSerializerOptions)" />
    public T? Deserialize<T>(JsonElement element);
    
    /// <inheritdoc cref="JsonSerializer.Deserialize(JsonElement, Type, JsonSerializerOptions)" />
    public object? Deserialize(JsonElement element, Type type);
    
    
    /// <inheritdoc cref="JsonSerializer.Deserialize{T}(JsonNode, JsonSerializerOptions)" />
    public T? Deserialize<T>(JsonNode node);
    
    /// <inheritdoc cref="JsonSerializer.Deserialize(JsonNode, Type, JsonSerializerOptions)" />
    public object? Deserialize(JsonNode node, Type type);
    
    
    /// <inheritdoc cref="JsonSerializer.Deserialize{T}(JsonDocument, JsonSerializerOptions)" />
    public T? Deserialize<T>(JsonDocument document);
    
    /// <inheritdoc cref="JsonSerializer.Deserialize(JsonDocument, Type, JsonSerializerOptions)" />
    public object? Deserialize(JsonDocument document, Type type);
    
    
    /// <inheritdoc cref="JsonSerializer.Deserialize{T}(Stream, JsonSerializerOptions)" />
    public T? Deserialize<T>(Stream stream);

    /// <inheritdoc cref="JsonSerializer.Deserialize(Stream, Type, JsonSerializerOptions)" />
    public object? Deserialize(Stream stream, Type type);
    

    /// <inheritdoc cref="JsonSerializer.DeserializeAsync{T}(Stream, JsonSerializerOptions, CancellationToken)" />
    public ValueTask<T?> DeserializeAsync<T>(Stream json, CancellationToken cancellationToken = default);
    
    /// <inheritdoc cref="JsonSerializer.DeserializeAsync(Stream, Type, JsonSerializerOptions, CancellationToken)" />
    public ValueTask<object?> DeserializeAsync(Stream json, Type type, CancellationToken cancellationToken = default);

    
    /// <inheritdoc cref="JsonSerializer.DeserializeAsyncEnumerable{T}(Stream, JsonSerializerOptions, CancellationToken)" />
    public IAsyncEnumerable<T?> DeserializeAsyncEnumerable<T>(Stream json, CancellationToken cancellationToken = default);

    
    /// <inheritdoc cref="JsonSerializer.Serialize{T}(T, JsonSerializerOptions)" />
    public string Serialize<T>(T? value);
    
    /// <inheritdoc cref="JsonSerializer.Serialize(object, Type, JsonSerializerOptions)" />
    public string Serialize(object? value, Type type);
    
    
    /// <inheritdoc cref="JsonSerializer.Serialize{T}(Stream, T, JsonSerializerOptions)" />
    public void Serialize<T>(Stream stream, T? value);
    
    /// <inheritdoc cref="JsonSerializer.Serialize(Stream, object, Type, JsonSerializerOptions)" />
    public void Serialize(Stream stream, object? value, Type type);

    
    /// <inheritdoc cref="JsonSerializer.Serialize{T}(Utf8JsonWriter, T, JsonSerializerOptions)" />
    public void Serialize<T>(Utf8JsonWriter writer, T? value);
    
    /// <inheritdoc cref="JsonSerializer.Serialize(Utf8JsonWriter, object, Type, JsonSerializerOptions)" />
    public void Serialize(Utf8JsonWriter writer, object? value, Type type);
    
    
    /// <inheritdoc cref="JsonSerializer.SerializeToUtf8Bytes{T}(T, JsonSerializerOptions)" />
    public byte[] SerializeToUtf8Bytes<T>(T? value);
    
    /// <inheritdoc cref="JsonSerializer.SerializeToUtf8Bytes(object, Type, JsonSerializerOptions)" />
    public byte[] SerializeToUtf8Bytes(object? value, Type type);
    
    
    /// <inheritdoc cref="JsonSerializer.SerializeToElement{T}(T, JsonSerializerOptions)" />
    public JsonElement SerializeToElement<T>(T? value);
    
    /// <inheritdoc cref="JsonSerializer.SerializeToElement(object, Type, JsonSerializerOptions)" />
    public JsonElement SerializeToElement(object? value, Type type);
    
    
    /// <inheritdoc cref="JsonSerializer.SerializeToNode{T}(T, JsonSerializerOptions)" />
    public JsonNode? SerializeToNode<T>(T? value);
    
    /// <inheritdoc cref="JsonSerializer.SerializeToNode(object, Type, JsonSerializerOptions)" />
    public JsonNode? SerializeToNode(object? value, Type type);
    
    
    /// <inheritdoc cref="JsonSerializer.SerializeToDocument{T}(T, JsonSerializerOptions)" />
    public JsonDocument SerializeToDocument<T>(T? value);
    
    /// <inheritdoc cref="JsonSerializer.SerializeToDocument(object, Type, JsonSerializerOptions)" />
    public JsonDocument SerializeToDocument(object? value, Type type);
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
    public T? Deserialize<T>(ReadOnlySpan<char> json) => JsonSerializer.Deserialize<T>(json, SerializerOptions);

    /// <inheritdoc />
    public object? Deserialize(ReadOnlySpan<char> json, Type type) => JsonSerializer.Deserialize(json, type, SerializerOptions);

    /// <inheritdoc />
    public T? Deserialize<T>(ReadOnlySpan<byte> json) => JsonSerializer.Deserialize<T>(json, SerializerOptions);

    /// <inheritdoc />
    public object? Deserialize(ReadOnlySpan<byte> json, Type type) => JsonSerializer.Deserialize(json, type, SerializerOptions);

    /// <inheritdoc />
    public T? Deserialize<T>(ref Utf8JsonReader reader) => JsonSerializer.Deserialize<T>(ref reader, SerializerOptions);

    /// <inheritdoc />
    public object? Deserialize(ref Utf8JsonReader reader, Type type) => JsonSerializer.Deserialize(ref reader, type, SerializerOptions);

    /// <inheritdoc />
    public T? Deserialize<T>(JsonElement element) => element.Deserialize<T>(SerializerOptions);

    /// <inheritdoc />
    public object? Deserialize(JsonElement element, Type type) => element.Deserialize(type, SerializerOptions);

    /// <inheritdoc />
    public T? Deserialize<T>(JsonNode node) => node.Deserialize<T>(SerializerOptions);

    /// <inheritdoc />
    public object? Deserialize(JsonNode node, Type type) => node.Deserialize(type, SerializerOptions);

    /// <inheritdoc />
    public T? Deserialize<T>(JsonDocument document) => document.Deserialize<T>(SerializerOptions);

    /// <inheritdoc />
    public object? Deserialize(JsonDocument document, Type type) => document.Deserialize(type, SerializerOptions);


    /// <inheritdoc />
    public T? Deserialize<T>(Stream stream) => JsonSerializer.Deserialize<T>(stream, SerializerOptions);
    
    /// <inheritdoc />
    public object? Deserialize(Stream stream, Type type) => JsonSerializer.Deserialize(stream, type, SerializerOptions);
    
    /// <inheritdoc />
    public ValueTask<T?> DeserializeAsync<T>(Stream json, CancellationToken cancellationToken = default)
        => JsonSerializer.DeserializeAsync<T>(json, SerializerOptions, cancellationToken);
    
    /// <inheritdoc />
    public ValueTask<object?> DeserializeAsync(Stream json, Type type, CancellationToken cancellationToken = default)
        => JsonSerializer.DeserializeAsync(json, type, SerializerOptions, cancellationToken);
    
    /// <inheritdoc />
    public IAsyncEnumerable<T?> DeserializeAsyncEnumerable<T>(Stream stream, CancellationToken cancellationToken = default)
        => JsonSerializer.DeserializeAsyncEnumerable<T>(stream, SerializerOptions, cancellationToken);
    
    /// <inheritdoc />
    public string Serialize<T>(T? value) => JsonSerializer.Serialize(value, SerializerOptions);
    
    /// <inheritdoc />
    public string Serialize(object? value, Type type) => JsonSerializer.Serialize(value, type, SerializerOptions);

    /// <inheritdoc />
    public void Serialize<T>(Stream stream, T? value) => JsonSerializer.Serialize(stream, value, SerializerOptions);

    /// <inheritdoc />
    public void Serialize(Stream stream, object? value, Type type) => JsonSerializer.Serialize(value, type, SerializerOptions);

    /// <inheritdoc />
    public void Serialize<T>(Utf8JsonWriter writer, T? value) => JsonSerializer.Serialize(writer, value, SerializerOptions);

    /// <inheritdoc />
    public void Serialize(Utf8JsonWriter writer, object? value, Type type) => JsonSerializer.Serialize(value, type, SerializerOptions);

    /// <inheritdoc />
    public byte[] SerializeToUtf8Bytes<T>(T? value) => JsonSerializer.SerializeToUtf8Bytes(value, SerializerOptions);

    /// <inheritdoc />
    public byte[] SerializeToUtf8Bytes(object? value, Type type) => JsonSerializer.SerializeToUtf8Bytes(value, type, SerializerOptions);
    
    /// <inheritdoc />
    public JsonElement SerializeToElement<T>(T? value) => JsonSerializer.SerializeToElement(value, SerializerOptions);

    /// <inheritdoc />
    public JsonElement SerializeToElement(object? value, Type type) => JsonSerializer.SerializeToElement(value, type, SerializerOptions);

    /// <inheritdoc />
    public JsonNode? SerializeToNode<T>(T? value) => JsonSerializer.SerializeToNode(value, SerializerOptions);

    /// <inheritdoc />
    public JsonNode? SerializeToNode(object? value, Type type) => JsonSerializer.SerializeToNode(value, type, SerializerOptions);

    /// <inheritdoc />
    public JsonDocument SerializeToDocument<T>(T? value) => JsonSerializer.SerializeToDocument(value, SerializerOptions);

    /// <inheritdoc />
    public JsonDocument SerializeToDocument(object? value, Type type) => JsonSerializer.SerializeToDocument(value, type, SerializerOptions);
}