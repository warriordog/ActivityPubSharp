// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using ActivityPub.Types.Conversion.Overrides;
using InternalUtils;

namespace ActivityPub.Types.Internal;

internal interface ISubTypePivot
{
    bool TryNarrowType(Type entityType, JsonElement jsonElement, DeserializationMetadata meta, [NotNullWhen(true)] out Type? narrowType);
}

internal class SubTypePivot : ISubTypePivot
{
    private readonly Dictionary<Type, TypeSelector?> _typeSelectorCache = new();
    
    private readonly Func<Type, TypeSelector> _createTypeSelector =
        typeof(SubTypePivot)
        .GetRequiredMethod(nameof(CreateTypeSelector), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
        .CreateGenericPivot<TypeSelector>();

    public bool TryNarrowType(Type entityType, JsonElement jsonElement, DeserializationMetadata meta, [NotNullWhen(true)] out Type? narrowType)
    {
        var typeSelector = GetTypeSelector(entityType);
        
        if (typeSelector != null)
            return typeSelector.TryNarrowType(jsonElement, meta, out narrowType);
        
        narrowType = null;
        return false;
    }
    
    private TypeSelector? GetTypeSelector(Type type)
    {
        if (!_typeSelectorCache.TryGetValue(type, out var selector))
        {
            if (type.IsAssignableTo(typeof(ISubTypeDeserialized)))
                selector = _createTypeSelector(type);
            
            _typeSelectorCache[type] = selector;
        }

        return selector;
    }

    private static TypeSelector CreateTypeSelector<TEntity>()
        where TEntity : ISubTypeDeserialized
        => new TypeSelector<TEntity>();

    private abstract class TypeSelector
    {
        public abstract bool TryNarrowType(JsonElement element, DeserializationMetadata meta, [NotNullWhen(true)] out Type? type);
    }

    private class TypeSelector<T> : TypeSelector
        where T : ISubTypeDeserialized
    {
        public override bool TryNarrowType(JsonElement element, DeserializationMetadata meta, [NotNullWhen(true)] out Type? type)
            => T.TryNarrowTypeByJson(element, meta, out type);
    }
}