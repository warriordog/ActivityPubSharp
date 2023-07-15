using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using JetBrains.Annotations;

namespace ActivityPub.Types.Json;

/// <summary>
/// Indicates that the target method should be called to serialize this type.
/// Only valid on subtypes of <see cref="ASType"/>.
/// Target method must be public, static, and conform to the signature of <see cref="TrySerializeDelegate{T}"/> where T is substituted for the containing type.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
[MeansImplicitUse]
public sealed class CustomJsonSerializerAttribute : Attribute {}

/// <summary>
/// Serialize the type into JSON.
/// Return true on success, or false to fall back on the default converter. 
/// </summary>
/// <typeparam name="T">Type of object to convert</typeparam>
public delegate bool TrySerializeDelegate<in T>(T obj, JsonSerializerOptions options, JsonNodeOptions nodeOptions, [NotNullWhen(true)] out JsonNode? node);