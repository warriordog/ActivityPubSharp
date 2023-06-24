// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace ActivityPub.Types.Internal;

// Yeah yeah, this doesn't follow naming conventions.
// But I'm not about to make one of these for each and every json class we might need to extend.
internal static class JsonExtensions
{
    /// <summary>
    /// Checks if a JSON reader is pointed to an ActivityStreams object.
    /// If so, returns true and provides the AS "type" value.
    /// </summary>
    /// <param name="reader">Reader to scan from. Intentionally NOT passed by ref, to ensure we get a copy.</param>
    /// <param name="type">Extracted type string. NOT normalized - could be in the wrong case!</param>
    /// <returns>Returns true if the reader is pointed as an AS object, false otherwise</returns>
    public static bool TryGetASObjectType(this Utf8JsonReader reader, [NotNullWhen(true)] out string? type)
    {
        type = null;

        // If its not an object, then its not an object :shrug:
        if (reader.TokenType != JsonTokenType.StartObject)
            return false;

        // Read exactly one object (plus nested objects)
        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            switch (reader.TokenType)
            {
                // Skip any nested objects
                case JsonTokenType.StartObject:
                {
                    reader.SkipUntil(JsonTokenType.EndObject);
                    break;
                }

                // Skip any nested arrays
                case JsonTokenType.StartArray:
                {
                    reader.SkipUntil(JsonTokenType.EndArray);
                    break;
                }

                // Read the "type" property
                case JsonTokenType.PropertyName when reader.GetString() == "type":
                {
                    // Move to property value (type name or array)
                    reader.Read();

                    // If its a string, then compare it directly
                    if (reader.TryGetString(out type))
                        return true;

                    // If its an array, then we need to scan through all contents
                    if (reader.TokenType == JsonTokenType.StartArray)
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            if (reader.TryGetString(out type))
                                return true;

                    // If we get here, then type wasn't in any valid format
                    return false;
                }
            }
        }

        // If we get here, then something failed and/or this is NOT an AS object
        return false;
    }

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
    /// Progresses a JSON reader until a specified token is reached or the stream ends. 
    /// </summary>
    /// <param name="reader">Reader to progress</param>
    /// <param name="tokenType">Token to check for</param>
    /// <returns>True if the token was found, false if the stream ended</returns>
    public static bool SkipUntil(this ref Utf8JsonReader reader, JsonTokenType tokenType)
    {
        while (reader.Read() && reader.TokenType != tokenType)
        {
            // no-op: everything happens in the loop condition above
        }

        // Final check for success or failure result
        return reader.TokenType == tokenType;
    }
}