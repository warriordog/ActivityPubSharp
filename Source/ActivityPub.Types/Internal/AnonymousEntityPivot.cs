// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;
using System.Text.Json;
using ActivityPub.Types.Conversion.Overrides;
using InternalUtils;

namespace ActivityPub.Types.Internal;

internal interface IAnonymousEntityPivot
{
    bool ShouldConvert(Type entityType, JsonElement jsonElement);
}

internal class AnonymousEntityPivot : IAnonymousEntityPivot
{
    private readonly Dictionary<Type, AnonymousChecker> _anonymousCheckerCache = new();
    private readonly Func<Type, AnonymousChecker> _createAnonymousChecker =
        typeof(AnonymousEntityPivot)
        .GetRequiredMethod(nameof(CreateAnonymousChecker), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
        .CreateGenericPivot<AnonymousChecker>();

    public bool ShouldConvert(Type entityType, JsonElement jsonElement)
    {
        var checker = GetAnonymousChecker(entityType);
        return checker.ShouldConvert(jsonElement);
    }
    
    private AnonymousChecker GetAnonymousChecker(Type entityType)
    {
        if (!_anonymousCheckerCache.TryGetValue(entityType, out var checker))
        {
            checker = _createAnonymousChecker(entityType);
            _anonymousCheckerCache[entityType] = checker;
        }

        return checker;
    }
    
    private static AnonymousChecker CreateAnonymousChecker<TEntity>()
        where TEntity : IAnonymousEntity
        => new AnonymousChecker<TEntity>();
    
    private abstract class AnonymousChecker
    {
        public abstract bool ShouldConvert(JsonElement jsonElement);
    }

    private class AnonymousChecker<T> : AnonymousChecker
        where T : IAnonymousEntity
    {
        public override bool ShouldConvert(JsonElement jsonElement)
            => T.ShouldConvertFrom(jsonElement);
    }
}