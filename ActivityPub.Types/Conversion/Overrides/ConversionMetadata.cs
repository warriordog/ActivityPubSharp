// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Nodes;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Conversion.Overrides;

/// <summary>
///     Context for a particular JSON conversion operation.
///     Singleton - created once and reused for entire graph.
/// </summary>
public class ConversionMetadata
{
    /// <summary>
    ///     JSON serializer options in use for the conversion.
    ///     MUST be passed on - do not assume default values!
    /// </summary>
    public required JsonSerializerOptions JsonSerializerOptions { get; init; }
}

/// <summary>
///     Context for a particular JSON serialization operation.
///     Singleton - created once and reused for entire graph.
/// </summary>
public class SerializationMetadata : ConversionMetadata
{
    /// <summary>
    ///     JSON node options in use for the conversion.
    ///     MUST be passed on - do not assume default values!
    /// </summary>
    public required JsonNodeOptions JsonNodeOptions { get; init; }
}

/// <summary>
///     Context for a particular JSON deserialization operation.
///     Singleton - created once and reused for entire graph.
/// </summary>
public class DeserializationMetadata : ConversionMetadata
{
    /// <summary>
    ///     TypeMap of the object being converted.
    /// </summary>
    public required TypeMap TypeMap { get; init; }

    /// <summary>
    ///     JSON-LD context in effect for this conversion.
    ///     Will always be present, even if not included in the JSON.
    /// </summary>
    public required JsonLDContext LDContext { get; init; }
}