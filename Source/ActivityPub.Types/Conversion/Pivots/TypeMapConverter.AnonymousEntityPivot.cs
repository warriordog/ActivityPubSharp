// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

// ReSharper disable CheckNamespace

using System.Reflection;
using System.Text.Json;
using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Conversion.Pivots;
using ActivityPub.Types.Internal;

namespace ActivityPub.Types.Conversion.Converters;

public partial class TypeMapConverter
{
    internal class AnonymousEntityPivot : IAnonymousEntityPivot
    {
        private readonly Dictionary<Type, AnonymousChecker> _anonymousCheckerCache = new();
        private readonly Func<Type, AnonymousChecker> _createAnonymousChecker =
            typeof(AnonymousEntityPivot)
                .GetRequiredMethod(nameof(CreateAnonymousChecker), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .CreateGenericPivotFunc<AnonymousChecker>();

        public bool ShouldConvert(Type entityType, JsonElement jsonElement, DeserializationMetadata meta)
        {
            var checker = GetAnonymousChecker(entityType);
            return checker.ShouldConvert(jsonElement, meta);
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
            public abstract bool ShouldConvert(JsonElement jsonElement, DeserializationMetadata meta);
        }

        private class AnonymousChecker<T> : AnonymousChecker
            where T : IAnonymousEntity
        {
            public override bool ShouldConvert(JsonElement jsonElement, DeserializationMetadata meta)
                => T.ShouldConvertFrom(jsonElement, meta);
        }
    }

}