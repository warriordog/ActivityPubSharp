// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using JetBrains.Annotations;

namespace ActivityPub.Types.Attributes;

/// <summary>
///     When placed on an entity deriving from <see cref="ASEntity" />, indicates that any object containing this type will implicitly include another type.
///     This is only used during JSON conversion, so the public inheritance chain SHOULD follow the same structure.
///     Multiple attributes may be specified.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
[BaseTypeRequired(typeof(ASEntity))]
public sealed class ImpliesOtherEntityAttribute : Attribute
{
    /// <summary>
    ///     Type of the other entity.
    /// </summary>
    public readonly Type Type;

    public ImpliesOtherEntityAttribute(Type type)
    {
        if (!type.IsAssignableTo(typeof(ASEntity)))
            throw new ArgumentException("Implied type must derive from ASBase", nameof(type));

        Type = type;
    }
}