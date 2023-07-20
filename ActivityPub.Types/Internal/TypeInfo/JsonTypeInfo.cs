﻿// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Internal.TypeInfo;

/// <summary>
/// Cached JSON serialization info for a particular type
/// </summary>
public abstract class JsonTypeInfo
{
    /// <summary>
    /// Type that this object describes
    /// </summary>
    public required Type Type { get; init; }

    /// <summary>
    /// All settable & required JSON properties in the type.
    /// These will always be parsed from JSON.
    /// </summary>
    /// <remarks>
    /// RequiredSetters and OptionalSetters are separated to allow efficient single-pass parsing.
    /// </remarks>
    /// <seealso cref="OptionalSetters"/>
    /// <seealso cref="Getters"/>
    public required Dictionary<string, JsonPropertyInfo> RequiredSetters { get; init; }
    
    /// <summary>
    /// All settable & optional JSON properties in the type.
    /// These will be parsed from JSON only if present.
    /// </summary>
    /// <remarks>
    /// RequiredSetters and OptionalSetters are separated to allow efficient single-pass parsing.
    /// </remarks>
    /// <seealso cref="RequiredSetters"/>
    /// <seealso cref="Getters"/>
    public required Dictionary<string, JsonPropertyInfo> OptionalSetters { get; init; }

    /// <summary>
    /// All readable JSON properties in the type.
    /// These will be serialized to JSON
    /// </summary>
    /// <seealso cref="OptionalSetters"/>
    /// <seealso cref="RequiredSetters"/>
    public required JsonPropertyInfo[] Getters { get; init; }

    /// <summary>
    /// Attempt to serialize an instance of <see cref="Type"/> using its <see cref="TrySerializeDelegate{T}"/>.
    /// If the type has no custom serializer, then false is returned.
    /// False will trigger the default serializer to process <see cref="obj"/>.
    /// </summary>
    /// <param name="obj">Instance to serialize</param>
    /// <param name="options">JSON options - MUST be preserved downstream!</param>
    /// <param name="nodeOptions">For convenience - JsonNodeOptions pre-created from <see cref="options"/>.</param>
    /// <param name="node">Output node to contain the resulting JSON</param>
    /// <returns>Returns true on success, false otherwise.</returns>
    public abstract bool TrySerialize(object obj, JsonSerializerOptions options, JsonNodeOptions nodeOptions, [NotNullWhen(true)] out JsonNode? node);

    /// <summary>
    /// Attempt to serialize an instance of <see cref="Type"/> using its <see cref="TryDeserializeDelegate{T}"/>.
    /// If the type has no custom deserializer, then false is returned.
    /// False will trigger the default deserializer to process <see cref="obj"/>.
    /// </summary>
    /// <param name="element">JSON element to parse</param>
    /// <param name="options">JSON options - MUST be preserved downstream!</param>
    /// <param name="obj">Output object to contain the resulting instance</param>
    /// <returns>Returns true on success, false otherwise</returns>
    public abstract bool TryDeserialize(JsonElement element, JsonSerializerOptions options, out object? obj);
}

internal class JsonTypeInfo<T> : JsonTypeInfo
{
    public required TrySerializeDelegate<T>? CustomSerializer { get; init; }
    public required TryDeserializeDelegate<T>? CustomDeserializer { get; init; }


    public override bool TrySerialize(object obj, JsonSerializerOptions options, JsonNodeOptions nodeOptions, [NotNullWhen(true)] out JsonNode? node)
    {
        if (CustomSerializer != null)
            return CustomSerializer((T)obj, options, nodeOptions, out node);

        node = null;
        return false;
    }

    public override bool TryDeserialize(JsonElement element, JsonSerializerOptions options, out object? obj)
    {
        if (CustomDeserializer != null && CustomDeserializer(element, options, out var typedObj))
        {
            obj = typedObj;
            return true;
        }

        obj = null;
        return false;
    }
}