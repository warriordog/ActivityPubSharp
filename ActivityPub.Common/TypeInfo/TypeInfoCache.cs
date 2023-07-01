// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Internal;
using ActivityPub.Types.Util;

namespace ActivityPub.Common.TypeInfo;

public class TypeInfoCache : ITypeInfoCache
{
    private readonly Dictionary<Type, TypeInfo> _infoCache = new();

    public TypeInfo GetFor(Type type)
    {
        if (!_infoCache.TryGetValue(type, out var info))
        {
            info = CreateInfoFor(type);
            _infoCache[type] = info;
        }

        return info;
    }

    private static TypeInfo CreateInfoFor(Type type) => new()
    {
        Type = type,
        LinkableProperties = GetPropertiesOfGenericType(type, typeof(Linkable<>)),
        LinkableListProperties = GetPropertiesOfGenericType(type, typeof(LinkableList<>))
    };

    // This is a bit weird because we need an efficiently packed array of structs
    private static GenericPropInfo[] GetPropertiesOfGenericType(Type type, Type genericType)
    {
        if (!genericType.IsGenericType)
            throw new ArgumentException("genericType must be a generic", nameof(genericType));

        var props = type
            .GetProperties()
            .Where(prop =>
                prop.PropertyType.IsConstructedGenericType &&
                prop.PropertyType.IsAssignableToGenericType(genericType))
            .ToList();

        var info = new GenericPropInfo[props.Count];
        for (var i = 0; i < info.Length; i++)
        {
            var prop = props[i];
            var propGeneric = prop.PropertyType.GetGenericArguments()[0];
            info[i] = new GenericPropInfo(prop, propGeneric);
        }

        return info;
    }
}