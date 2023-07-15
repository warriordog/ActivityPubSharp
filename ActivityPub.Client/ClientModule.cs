// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace ActivityPub.Client;

public static class ClientModule
{
    public static void TryAddClientModule(this HostApplicationBuilder builder)
    {
        builder.Services.TryAddCommonModule();
        builder.Services.TryAddSingleton<IActivityPubClient, ActivityPubClient>();
    }
}