// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;

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
    internal static bool TryGetString(this Utf8JsonReader reader, [NotNullWhen(true)] out string? type)
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
    /// Attempts to read the element as a string.
    /// Returns true on success.
    /// </summary>
    /// <param name="element">Element to convert</param>
    /// <param name="str">String that was read</param>
    /// <returns>True if a string was read, false otherwise</returns>
    internal static bool TryGetString(this JsonElement element, [NotNullWhen(true)] out string? str)
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
    /// <param name="element">object to read</param>
    /// <param name="type">Set to the AS type on success, or null on failure</param>
    /// <returns>Returns true on success, false on failure</returns>
    internal static bool TryGetASType(this JsonElement element, [NotNullWhen(true)] out string? type)
    {
        if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty("type", out var asTypeElement) && asTypeElement.TryGetString(out type))
            return true;

        type = null;
        return false;
    }

    /// <summary>
    /// Converts a JsonElement to the appropriate JsonNode subtype.
    /// </summary>
    /// <param name="element">Element to convert</param>
    /// <param name="options">Optional options to pass to JsonNode</param>
    /// <returns>Node containing the same information</returns>
    internal static JsonNode? ToNode(this JsonElement element, JsonNodeOptions? options = null) => element.ValueKind switch
    {
        JsonValueKind.Array => JsonArray.Create(element, options),
        JsonValueKind.Object => JsonObject.Create(element, options),
        _ => JsonValue.Create(element, options)
    };
}