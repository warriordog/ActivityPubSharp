// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using JetBrains.Annotations;

namespace ActivityPub.Types.Json;

/// <summary>
/// Indicates that the target method should be called to deserialize this type.
/// Only valid on subtypes of <see cref="ASType"/>.
/// Target method must be public, static, and conform to the signature of <see cref="TryDeserializeDelegate{T}"/> where T is substituted for the containing type.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
[MeansImplicitUse]
public sealed class CustomJsonDeserializerAttribute : Attribute {}

/// <summary>
/// Deserialize the type from JSON.
/// Return true on success, or false to fall back on the default converter. 
/// </summary>
/// <typeparam name="T">Type of object to convert</typeparam>
public delegate bool TryDeserializeDelegate<T>(JsonElement element, JsonSerializerOptions options, out T? obj);