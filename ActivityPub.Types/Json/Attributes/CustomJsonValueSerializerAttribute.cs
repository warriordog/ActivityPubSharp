// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using JetBrains.Annotations;

namespace ActivityPub.Types.Json.Attributes;

/// <summary>
/// Indicates that the target method should be called to serialize this type into a non-object value.
/// Only valid on subtypes of <see cref="ASBase"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
[MeansImplicitUse]
public sealed class CustomJsonValueSerializerAttribute : Attribute
{
    /// <summary>
    /// Name of the method that can serialize this type.
    /// Must be public, static, and conform to the signature of <see cref="TrySerializeIntoValueDelegate{T}"/> where T is substituted for the type.
    /// </summary>
    public string MethodName { get; }

    public CustomJsonValueSerializerAttribute(string methodName) => MethodName = methodName;
}

/// <summary>
/// Serialize the type into a JSON value.
/// This will supersede all conversion for the ENTIRE object graph, so use it carefully!
/// </summary>
/// <param name="obj">Object to convert</param>
/// <param name="meta">Context for the conversion</param>
/// <param name="node">Node to use as value for the object</param>
/// <returns>Return true on success, or false to fall back on default logic.</returns>
/// <typeparam name="T">Type of object to convert. Must derive from <see cref="ASBase"/>.</typeparam>
public delegate bool TrySerializeIntoValueDelegate<in T>(T obj, SerializationMetadata meta, [NotNullWhen(true)] out JsonValue? node)
    where T : ASBase;