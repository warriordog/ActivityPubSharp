// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using JetBrains.Annotations;

namespace ActivityPub.Types.Attributes;

/// <summary>
///     Registers the class as an entity that handles a specific ActivityStreams type.
///     The type name must be unique.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
[BaseTypeRequired(typeof(ASEntity))]
[MeansImplicitUse]
public sealed class APConvertibleAttribute : Attribute
{
    /// <summary>
    ///     ActivityStreams type name
    /// </summary>
    public readonly string Type;

    /// <summary>
    ///     JSON-LD context that defines this type.
    ///     Will be used for conversion.
    ///     Defaults to the base ActivityStreams context.
    /// </summary>
    public string? Context { get; init; }

    public APConvertibleAttribute(string type) => Type = type;
}