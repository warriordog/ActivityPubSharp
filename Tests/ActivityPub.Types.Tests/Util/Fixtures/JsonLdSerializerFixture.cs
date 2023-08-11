// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Conversion;
using ActivityPub.Types.Internal;
using JetBrains.Annotations;

namespace ActivityPub.Types.Tests.Util.Fixtures;

/// <summary>
///     Provides a pre-initialized <see cref="IJsonLdSerializer" /> instance for use in tests.
///     All loaded assemblies are registered.
///     This should be used to avoid the processing load of populating the caches from scratch on every single test.
/// </summary>
/// <seealso href="https://xunit.net/docs/shared-context#collection-fixture" />
[UsedImplicitly]
public sealed class JsonLdSerializerFixture
{
    public JsonLdSerializerFixture()
    {
        ASTypeInfoCache = new ASTypeInfoCache();
        ASTypeInfoCache.RegisterAllAssemblies();

        JsonLdSerializer = new JsonLdSerializer(ASTypeInfoCache);
    }

    public IASTypeInfoCache ASTypeInfoCache { get; }
    public IJsonLdSerializer JsonLdSerializer { get; }
}