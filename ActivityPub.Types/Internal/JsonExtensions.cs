// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ActivityPub.Types.Internal;

// Yeah yeah, this doesn't follow naming conventions.
// But I'm not about to make one of these for each and every json class we might need to extend.
internal static class JsonExtensions
{
    /// <summary>
    /// Attempts to read a string from the provided JSON reader.
    /// Returns false + null if the reader is not positioned at a string or reading fails.
    /// Does NOT advance the reader!
    /// </summary>
    /// <param name="reader">Reader, not copied because we don't move it</param>
    /// <param name="type">String that was read</param>
    /// <returns>Returns true if a string was read, false otherwise.</returns>
    public static bool TryGetString(this Utf8JsonReader reader, [NotNullWhen(true)] out string? type)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            type = null;
            return false;
        }

        type = reader.GetString();
        return type != null;
    }

    /// <summary>
    /// Modifies the JsonSerializerOptions to remove all JsonConverters of the specified type
    /// </summary>
    /// <param name="options"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Returns the same object for chaining</returns>
    public static JsonSerializerOptions RemoveConvertersOfType<T>(this JsonSerializerOptions options)
        where T : JsonConverter
    {
        options.Converters.RemoveWhere(c => c is T);
        return options;
    }

    /// <summary>
    /// Attempts to read the element as a string.
    /// Returns true on success.
    /// </summary>
    /// <param name="element">Element to convert</param>
    /// <param name="str">String that was read</param>
    /// <returns>True if a string was read, false otherwise</returns>
    public static bool TryGetString(this JsonElement element, [NotNullWhen(true)] out string? str)
    {
        if (element.ValueKind != JsonValueKind.String)
        {
            str = null;
            return false;
        }

        str = element.GetString();
        return str != null;
    }

    /// <summary>
    /// Attempts to read the provided <see cref="JsonElement"/> as an ActivityStreams object and return its type.
    /// Returns false if the element does not contain an object or the object does not contain a valid type.
    /// </summary>
    /// <param name="objectElement">object to read</param>
    /// <param name="type">Set to the AS type on success, or null on failure</param>
    /// <returns>Returns true on success, false on failure</returns>
    public static bool TryGetASType(this JsonElement objectElement, [NotNullWhen(true)] out string? type)
    {
        if (objectElement.TryGetProperty("type", out var asTypeElement) && asTypeElement.TryGetString(out type))
            return true;

        type = null;
        return false;
    }

    // /// <summary>
    // /// TODO docs
    // /// </summary>
    // /// <param name="element"></param>
    // /// <param name="name"></param>
    // /// <param name="options"></param>
    // /// <param name="value"></param>
    // /// <typeparam name="T"></typeparam>
    // /// <returns></returns>
    // /// <exception cref="JsonException"></exception>
    // public static bool TryGetProperty<T>(this JsonElement element, string name, JsonSerializerOptions options, [NotNullWhen(true)] out T? value)
    // {
    //     if (!element.TryGetProperty(name, out var prop))
    //     {
    //         value = default;
    //         return false;
    //     }
    //
    //     value = prop.Deserialize<T>() ?? throw new JsonException($"Conversion to {typeof(T)} failed");
    //     return true;
    // }
}