// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;

namespace ActivityPub.Types.Conversion;

/// <summary>
///     Configuration options for <see cref="JsonLdSerializer"/>.
/// </summary>
public class JsonLdSerializerOptions
{
    /// <summary>
    ///     JSON options that will be merged with those used by the library.
    ///     These act as a base layer, and required changes will be automatically overlaid by the library. 
    /// </summary>
    /// <remarks>
    ///     The default value is "new(JsonSerializerOptions.Default)" to allow application code to modify it in a services.Configure() callback.
    /// </remarks>
    public JsonSerializerOptions DefaultJsonSerializerOptions { get; set; } = new(JsonSerializerOptions.Default);
}