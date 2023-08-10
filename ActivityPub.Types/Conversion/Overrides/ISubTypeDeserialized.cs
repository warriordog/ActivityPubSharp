// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;

namespace ActivityPub.Types.Conversion.Overrides;

/// <summary>
///     Indicates that the type can deserialize into a subtype of itself.
/// </summary>
public interface ISubTypeDeserialized
{
    /// <summary>
    ///     Selects a more narrow type to convert instead of the containing type.
    ///     This will be called on deserialization.
    ///     Implementations should change the value of type and return true.
    /// </summary>
    /// <param name="element">Element containing JSON data for this object</param>
    /// <param name="meta">Context for the conversion</param>
    /// <param name="type">The type that will be constructed</param>
    /// <returns>True if type was changed, false otherwise</returns>
    public static abstract bool TryNarrowTypeByJson(JsonElement element, DeserializationMetadata meta, ref Type type);
}