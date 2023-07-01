// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;

namespace ActivityPub.Common.TypeInfo;

public class TypeInfo
{
    public required Type Type { get; init; }

    public required GenericPropInfo[] LinkableProperties { get; init; }
    public required GenericPropInfo[] LinkableListProperties { get; init; }
}

public readonly record struct GenericPropInfo(PropertyInfo Property, Type GenericType);