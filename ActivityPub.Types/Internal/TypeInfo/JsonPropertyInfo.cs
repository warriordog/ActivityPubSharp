// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Internal.TypeInfo;

/// <summary>
/// Cached details about a particular JSON property
/// </summary>
public abstract class JsonPropertyInfo
{
    public required PropertyInfo Property { get; init; }
    public required string Name { get; init; }
    public required bool IsRequired { get; init; }
    
    /// <summary>
    /// Property-specific ignore condition.
    /// Will never be <see cref="JsonIgnoreCondition.Always"/>.
    /// </summary>
    /// <seealso cref="JsonIgnoreAttribute"/>
    public required JsonIgnoreCondition? IgnoreCondition { get; init; }
    
    /// <summary>
    /// True if the property should be ignored when nested inside two or more objects.
    /// </summary>
    /// <seealso cref="JsonIgnoreWhenNestedAttribute"/>
    public required bool IgnoreWhenNested { get; init; }
    public required object? TypeDefaultValue { get; init; }
    public required bool IsCollection { get; init; }
    
    /// <summary>
    /// Optional custom JSON converter to use for the property
    /// </summary>
    /// <seealso cref="JsonConverterAttribute"/>
    public required Type? CustomConverterType { get; init; }

    /// <summary>
    /// Attempt to serialize <see cref="Property"/> using its <see cref="TrySerializeDelegate{T}"/>.
    /// If the property has no custom serializer, then false is returned.
    /// False will trigger the default serializer to process <see cref="value"/>.
    /// </summary>
    /// <param name="value">Value to serialize</param>
    /// <param name="options">JSON options - MUST be preserved downstream!</param>
    /// <param name="nodeOptions">For convenience - JsonNodeOptions pre-created from <see cref="options"/>.</param>
    /// <param name="node">Output node to contain the resulting JSON</param>
    /// <returns>Returns true on success, false otherwise.</returns>
    public abstract bool TrySerialize(object value, JsonSerializerOptions options, JsonNodeOptions nodeOptions, [NotNullWhen(true)] out JsonNode? node);

    /// <summary>
    /// Attempt to deserialize <see cref="Property"/> using its <see cref="TryDeserializeDelegate{T}"/>.
    /// If the property has no custom deserializer, then false is returned.
    /// False will trigger the default deserializer to process <see cref="value"/>.
    /// </summary>
    /// <param name="element">JSON element to parse</param>
    /// <param name="options">JSON options - MUST be preserved downstream!</param>
    /// <param name="value">Output object to contain the resulting value</param>
    /// <returns>Returns true on success, false otherwise</returns>
    public abstract bool TryDeserialize(JsonElement element, JsonSerializerOptions options, out object? value);
}

internal class JsonPropertyInfo<T> : JsonPropertyInfo
{
    public required TrySerializeDelegate<T>? CustomSerializer { get; init; }
    public required TryDeserializeDelegate<T>? CustomDeserializer { get; init; }


    public override bool TrySerialize(object value, JsonSerializerOptions options, JsonNodeOptions nodeOptions, [NotNullWhen(true)] out JsonNode? node)
    {
        if (CustomSerializer != null)
            return CustomSerializer((T)value, options, nodeOptions, out node);

        node = null;
        return false;
    }

    public override bool TryDeserialize(JsonElement element, JsonSerializerOptions options, out object? value)
    {
        if (CustomDeserializer != null && CustomDeserializer(element, options, out var typedObj))
        {
            value = typedObj;
            return true;
        }

        value = null;
        return false;
    }
}