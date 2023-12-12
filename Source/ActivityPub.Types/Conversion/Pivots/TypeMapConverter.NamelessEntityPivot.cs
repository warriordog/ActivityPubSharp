// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

// ReSharper disable CheckNamespace

using System.Reflection;
using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Conversion.Pivots;
using ActivityPub.Types.Internal;

namespace ActivityPub.Types.Conversion.Converters;

public partial class TypeMapConverter
{
    internal class NamelessEntityPivot : INamelessEntityPivot
    {
        private readonly Dictionary<Type, NamelessChecker> _namelessCheckerCache = new();
        private readonly Func<Type, NamelessChecker> _createNamelessChecker =
            typeof(NamelessEntityPivot)
                .GetRequiredMethod(nameof(CreateNamelessChecker), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .CreateGenericPivotFunc<NamelessChecker>();

        public bool ShouldConvert(Type entityType, DeserializationMetadata meta)
        {
            var checker = GetNamelessChecker(entityType);
            return checker.ShouldConvert(meta);
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
            public abstract bool ShouldConvert(DeserializationMetadata meta);
        }

        private class NamelessChecker<T> : NamelessChecker
            where T : INamelessEntity
        {
            public override bool ShouldConvert(DeserializationMetadata meta)
                => T.ShouldConvertFrom(meta.LDContext, meta);
        }
    }

}