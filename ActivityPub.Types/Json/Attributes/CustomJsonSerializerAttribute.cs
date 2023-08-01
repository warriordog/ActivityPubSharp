// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Nodes;
using JetBrains.Annotations;

namespace ActivityPub.Types.Json.Attributes;

/// <summary>
/// Indicates that the target method should be called to serialize this type or property.
/// Only valid on subtypes of <see cref="ASBase"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
[MeansImplicitUse]
public sealed class CustomJsonSerializerAttribute : Attribute
{
    /// <summary>
    /// Name of the method that can serialize this type or property.
    /// Must be public, static, and conform to the signature of <see cref="TrySerializeDelegate{T}"/> where T is substituted for the type or property type.
    /// </summary>
    public string MethodName { get; }

    public CustomJsonSerializerAttribute(string methodName) => MethodName = methodName;
}

/// <summary>
/// Serialize the type into JSON.
/// </summary>
/// <param name="obj">Object to convert</param>
/// <param name="meta">Context for the conversion</param>
/// <param name="node">Node to write values into</param>
/// <returns>Return true on success, or false to fall back on default logic.</returns>
/// <typeparam name="T">Type of object to convert. Must derive from <see cref="ASBase"/>.</typeparam>
public delegate bool TrySerializeDelegate<in T>(T obj, SerializationMetadata meta, JsonObject node)
    where T : ASBase;