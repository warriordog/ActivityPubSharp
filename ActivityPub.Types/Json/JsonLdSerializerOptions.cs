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

    public static JsonSerializerOptions AddJsonLd(this JsonSerializerOptions options)
    {
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.PropertyNameCaseInsensitive = true;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.Converters.Add(new ASTypeConverter());
        return options;
    }
}