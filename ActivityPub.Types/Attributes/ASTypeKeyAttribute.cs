// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using JetBrains.Annotations;

namespace ActivityPub.Types.Attributes;

/// <summary>
/// Registers the class as an entity that handles a specific ActivityStreams type.
/// The type name must be unique.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
[MeansImplicitUse]
public sealed class ASTypeKeyAttribute : Attribute
{
    /// <summary>
    /// ActivityStreams type name
    /// </summary>
    public readonly string Type;

    public ASTypeKeyAttribute(string type) => Type = type;
}