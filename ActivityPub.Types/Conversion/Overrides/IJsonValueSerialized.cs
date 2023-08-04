// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using ActivityPub.Types.AS;

namespace ActivityPub.Types.Conversion.Overrides;

/// <summary>
///     Indicates that this type is serialized into a JSON value instead of an object.
///     Within a single type graph, there can only be one type with this interface.
/// </summary>
/// <typeparam name="TThis">Type of object to convert</typeparam>
public interface IJsonValueSerialized<in TThis>
    where TThis : ASEntity
{
    /// <summary>
    ///     Serialize the type into a JSON value.
    ///     This will supersede all conversion for the ENTIRE object graph, so use it carefully!
    /// </summary>
    /// <param name="obj">Object to convert</param>
    /// <param name="meta">Context for the conversion</param>
    /// <param name="node">Node to use as value for the object</param>
    /// <returns>Return true on success, or false to fall back on default logic.</returns>
    public static abstract bool TrySerializeIntoValue(TThis obj, SerializationMetadata meta, [NotNullWhen(true)] out JsonValue? node);
}