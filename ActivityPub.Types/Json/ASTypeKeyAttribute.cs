// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Json;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class ASTypeKeyAttribute : Attribute
{
    /// <summary>
    /// ActivityStreams type name
    /// </summary>
    /// <seealso cref="ASType.Types"/>
    public readonly string Type;

    /// <summary>
    /// Set of types to use in place of <see cref="ASType"/> when <see cref="Type"/> is an open generic and type arguments cannot be inferred from usage.
    /// Irrelevant if Type is not an open generic.
    /// </summary>
    public readonly Type[]? DefaultGenerics;

    public ASTypeKeyAttribute(string type, Type[]? defaultGenerics = null)
    {
        Type = type;
        DefaultGenerics = defaultGenerics;
    }
}