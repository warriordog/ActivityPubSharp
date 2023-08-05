// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Nodes;
using ActivityPub.Types.AS;
using JetBrains.Annotations;

namespace ActivityPub.Types.Conversion.Overrides;

/// <summary>
///     Indicates that this type uses custom JSON serialization logic
/// </summary>
/// <typeparam name="TThis">Type of object to convert</typeparam>
[PublicAPI]
public interface ICustomJsonSerialized<in TThis> : ICustomJsonSerialized
    where TThis : ASEntity, ICustomJsonSerialized<TThis>, ICustomJsonSerialized
{
    /// <summary>
    ///     Serialize the type into JSON.
    /// </summary>
    /// <param name="obj">Object to convert</param>
    /// <param name="meta">Context for the conversion</param>
    /// <param name="node">Node to write values into</param>
    /// <returns>Return true on success, or false to fall back on default logic.</returns>
    public static abstract bool TrySerialize(TThis obj, SerializationMetadata meta, JsonObject node);

    bool ICustomJsonSerialized.TrySerialize(ASEntity obj, SerializationMetadata meta, JsonObject node)
        => TThis.TrySerialize((TThis)obj, meta, node);
}

/// <summary>
///     Internal adapter for non-generic callers.
/// </summary>
/// <seealso cref="ICustomJsonSerialized{TThis}"/>
public interface ICustomJsonSerialized
{
    /// <inheritdoc cref="ICustomJsonSerialized{TThis}.TrySerialize"/>
    /// <throws cref="InvalidCastException">If the input entity is not compatible with the implementation type</throws>
    internal bool TrySerialize(ASEntity obj, SerializationMetadata meta, JsonObject node);
}