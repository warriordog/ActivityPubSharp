// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Common.TypeInfo;
using ActivityPub.Common.Util;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace ActivityPub.Common;

public static class CommonModule
{
    public const string ConfigSection = "CommonModule";

    public static void TryAddCommonModule(this HostApplicationBuilder builder)
    {
        builder.Services.TryAddSingleton<ITypeInfoCache, TypeInfoCache>();
        builder.Services.TryAddSingleton<ActivityPubOptions>();
    }
}