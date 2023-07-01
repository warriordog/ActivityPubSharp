// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ActivityPub.Common.Extensions;

public static class ServiceCollectionExtensions
{
    // https://stackoverflow.com/questions/59293871/is-there-an-equivalent-to-iservicecollections-tryadd-methods-for-use-with-the
    // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-7.0#bind-hierarchical-configuration
    public static void TryConfigure<TOptions>(this HostApplicationBuilder builder, IConfigurationSection? configSection = null)
        where TOptions : class
    {
        // Skip if its already there
        if (builder.Services.Any(d => d.ServiceType == typeof(IConfigureOptions<TOptions>)))
            return;

        // Add the options
        configSection ??= builder.Configuration.GetSection(typeof(TOptions).Name);
        builder.Services.Configure<TOptions>(configSection);
    }
}