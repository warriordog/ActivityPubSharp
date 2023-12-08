﻿// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;
using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Internal.Pivots;

internal interface INamelessEntityPivot
{
    bool ShouldConvert(Type entityType, IJsonLDContext jsonLDContext);
}

internal class NamelessEntityPivot : INamelessEntityPivot
{
    private readonly Dictionary<Type, NamelessChecker> _namelessCheckerCache = new();
    private readonly Func<Type, NamelessChecker> _createNamelessChecker =
        typeof(NamelessEntityPivot)
            .GetRequiredMethod(nameof(CreateNamelessChecker), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
            .CreateGenericPivotFunc<NamelessChecker>();

    public bool ShouldConvert(Type entityType, IJsonLDContext jsonLDContext)
    {
        var checker = GetNamelessChecker(entityType);
        return checker.ShouldConvert(jsonLDContext);
    }
    
    private NamelessChecker GetNamelessChecker(Type entityType)
    {
        if (!_namelessCheckerCache.TryGetValue(entityType, out var checker))
        {
            checker = _createNamelessChecker(entityType);
            _namelessCheckerCache[entityType] = checker;
        }

        return checker;
    }
    
    private static NamelessChecker CreateNamelessChecker<TEntity>()
        where TEntity : INamelessEntity
        => new NamelessChecker<TEntity>();
    
    private abstract class NamelessChecker
    {
        public abstract bool ShouldConvert(IJsonLDContext jsonLDContext);
    }

    private class NamelessChecker<T> : NamelessChecker
        where T : INamelessEntity
    {
        public override bool ShouldConvert(IJsonLDContext jsonLDContext)
            => T.ShouldConvertFrom(jsonLDContext);
    }
}