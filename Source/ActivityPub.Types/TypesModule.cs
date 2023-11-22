// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Runtime.CompilerServices;
using ActivityPub.Types.Conversion;
using ActivityPub.Types.Conversion.Converters;
using ActivityPub.Types.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: InternalsVisibleTo("ActivityPub.Types.Tests")]

[assembly: InternalsVisibleTo("ActivityPub.Common")]
[assembly: InternalsVisibleTo("ActivityPub.Client")]
[assembly: InternalsVisibleTo("ActivityPub.Server")]
[assembly: InternalsVisibleTo("ActivityPub.Federation")]

namespace ActivityPub.Types;

/// <summary>
///     Dependency Injection for the ActivityPub.Types package.
/// </summary>
public static class TypesModule
{
    /// <summary>
    ///     Registers the module into a provided service collection.
    /// </summary>
    public static void TryAddTypesModule(this IServiceCollection services)
    {
        services.TryAddSingleton<INamelessEntityPivot, NamelessEntityPivot>();
        services.TryAddSingleton<IAnonymousEntityPivot, AnonymousEntityPivot>();
        services.TryAddSingleton<IJsonLdSerializer, JsonLdSerializer>();
        
        services.TryAddSingleton<IASTypeInfoCache>(
            _ =>
            {
                var cache = new ASTypeInfoCache();
                cache.RegisterAllAssemblies();
                return cache;
            }
        );
        
        services.AddSingleton<TypeMapConverter>();
        services.AddSingleton<ASTypeConverter>();
        services.AddSingleton<LinkableConverter>();
        services.AddSingleton<ListableConverter>();
        services.AddSingleton<ListableReadOnlyConverter>();
    }
}