// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Nodes;
using ActivityPub.Types.AS;

namespace ActivityPub.Types.Conversion.Overrides;

/// <summary>
///     Indicates that this type uses custom JSON serialization logic
/// </summary>
/// <typeparam name="TThis">Type of object to convert</typeparam>
public interface ICustomJsonSerialized<in TThis>
    where TThis : ASEntity
{
    /// <summary>
    ///     Serialize the type into JSON.
    /// </summary>
    /// <param name="obj">Object to convert</param>
    /// <param name="meta">Context for the conversion</param>
    /// <param name="node">Node to write values into</param>
    /// <returns>Return true on success, or false to fall back on default logic.</returns>
    public static abstract bool TrySerialize(TThis obj, SerializationMetadata meta, JsonObject node);
}