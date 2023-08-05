// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using ActivityPub.Types.AS;

namespace ActivityPub.Types.Conversion.Overrides;

/// <summary>
///     Indicates that this type uses custom JSON deserialization logic
/// </summary>
/// <typeparam name="TThis">Type of object to convert</typeparam>
public interface ICustomJsonDeserialized<TThis> : ICustomJsonDeserialized
    where TThis : ASEntity, ICustomJsonDeserialized<TThis>, ICustomJsonDeserialized
{
    /// <summary>
    ///     Deserialize the type from JSON.
    /// </summary>
    /// <param name="element">Element containing JSON data for this object</param>
    /// <param name="meta">Context for the conversion</param>
    /// <param name="obj">Object constructed by the converter</param>
    /// <returns>Return true on success, or false to fall back on default logic.</returns>
    /// <typeparam name="TThis">Type of object to convert. Must derive from <see cref="ASEntity" />.</typeparam>
    public static abstract bool TryDeserialize(JsonElement element, DeserializationMetadata meta, [NotNullWhen(true)] out TThis? obj);

    bool ICustomJsonDeserialized.TryDeserialize(JsonElement element, DeserializationMetadata meta, [NotNullWhen(true)] out ASEntity? obj)
    {
        if (TThis.TryDeserialize(element, meta, out var objT))
        {
            obj = objT;
            return true;
        }

        obj = null;
        return false;
    }
}

/// <summary>
///     Internal adapter for non-generic callers.
/// </summary>
/// <seealso cref="ICustomJsonDeserialized{TThis}"/>
public interface ICustomJsonDeserialized
{
    /// <inheritdoc cref="ICustomJsonDeserialized{TThis}.TryDeserialize"/>
    /// <throws cref="InvalidCastException">If the input entity is not compatible with the implementation type</throws>
    internal bool TryDeserialize(JsonElement element, DeserializationMetadata meta, [NotNullWhen(true)] out ASEntity? obj);
}