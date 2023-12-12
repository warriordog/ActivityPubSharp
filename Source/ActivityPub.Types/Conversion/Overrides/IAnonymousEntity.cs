// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;

namespace ActivityPub.Types.Conversion.Overrides;

/// <summary>
///     Indicates that the entity's presence in an object cannot be detected by any built-in method.
///     Exposes functionality to dynamically detect the entity within an arbitrary JSON message.
/// </summary>
/// <seealso cref="IAnonymousEntitySelector"/>
/// <seealso cref="INamelessEntity"/>
public interface IAnonymousEntity
{
    /// <summary>
    ///     Checks if this entity should be deserialized from the given JSON message.
    /// </summary>
    /// <param name="inputJson">JSON message that is being converted. May not be an object.</param>
    /// <param name="meta">Metadata that has already been parsed from the JSON.</param>
    public static abstract bool ShouldConvertFrom(JsonElement inputJson, DeserializationMetadata meta);
}