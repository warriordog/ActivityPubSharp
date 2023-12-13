// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ActivityPub.Client;

/// <summary>
///     Dependency injection for the <code>ActivityPub.Client</code> package
/// </summary>
public static class ClientModule
{
    /// <summary>
    ///     Registers the module into the provided service collection
    /// </summary>
    public static void TryAddClientModule(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddCommonModule();
        serviceCollection.TryAddSingleton<IActivityPubClient, ActivityPubClient>();
    }
}