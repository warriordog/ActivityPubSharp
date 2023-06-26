// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace ActivityPub.Types.Json;

// TODO create a non-static version with DI, so that application code can customize the options.

/// <summary>
/// Recommended JSON serializer options for working with JSON-LD
/// </summary>
public static class JsonLdSerializerOptions
{
    public static JsonSerializerOptions Default => new JsonSerializerOptions().AddJsonLd();

    /// <summary>
    /// Configures the JsonSerializerOptions for JSON-LD support
    /// </summary>
    /// <param name="options">Options to configure</param>
    /// <returns>Returns the same object for chaining</returns>
    public static JsonSerializerOptions AddJsonLd(this JsonSerializerOptions options)
    {
        AddFormatting(options);
        AddConverters(options);
        return options;
    }

    /// <summary>
    /// Configures settings for ideal JSON-LD compatibility
    /// </summary>
    /// <param name="options"></param>
    public static void AddFormatting(JsonSerializerOptions options)
    {
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.PropertyNameCaseInsensitive = true;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    }

    /// <summary>
    /// Registers polymorphic type converters to enable the full class hierarchy.
    /// </summary>
    /// <remarks> 
    /// Polymorphic converters must be added here for our pattern to work.
    /// If they're set in attributes, then there will be an infinite loop.
    /// </remarks>
    /// <param name="options"></param>
    public static void AddConverters(JsonSerializerOptions options)
    {
        options.Converters.Add(new ASTypeConverter());
        options.Converters.Add(new ASCollectionConverter());
    }
}