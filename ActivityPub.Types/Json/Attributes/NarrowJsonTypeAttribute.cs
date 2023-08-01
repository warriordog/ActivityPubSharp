// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using JetBrains.Annotations;

namespace ActivityPub.Types.Json.Attributes;

/// <summary>
/// Indicates that the target method should be called to narrow the type of this object before deserialization.
/// Only valid on subtypes of <see cref="ASBase"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
[MeansImplicitUse]
public sealed class NarrowJsonTypeAttribute : Attribute
{
    /// <summary>
    /// Name of the method that will narrow the type of this object
    /// Must be public, static, and conform to the signature of <see cref="NarrowTypeDelegate"/>.
    /// </summary>
    public string MethodName { get; }

    public NarrowJsonTypeAttribute(string methodName) => MethodName = methodName;
}

/// <summary>
/// Selects a more narrow type to convert instead of the containing type.
/// This will be called on deserialization.
/// The returned type MUST be or derive from the containing type.
/// </summary>
/// <param name="element">Element containing JSON data for this object</param>
/// <param name="meta">Context for the conversion</param>
/// <returns>Type of object to convert</returns>
public delegate Type NarrowTypeDelegate(JsonElement element, DeserializationMetadata meta);