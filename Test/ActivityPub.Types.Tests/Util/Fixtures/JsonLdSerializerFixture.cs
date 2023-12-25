// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Conversion;
using ActivityPub.Types.Conversion.Converters;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

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
        JsonLdSerializerOptions = new JsonLdSerializerOptions();
        var serializerOptions = Options.Create(JsonLdSerializerOptions);

        var typeMapConverter = new TypeMapConverter();

        var asTypeConverter = new ASTypeConverter();
        var linkableConverter = new LinkableConverter();
        var listableConverter = new ListableConverter();
        var listableReadOnlyConverter = new ListableReadOnlyConverter();
        JsonLdSerializer = new JsonLdSerializer(serializerOptions, typeMapConverter, asTypeConverter, linkableConverter, listableConverter, listableReadOnlyConverter);
    }

    internal IJsonLdSerializer JsonLdSerializer { get; }
    internal JsonLdSerializerOptions JsonLdSerializerOptions { get; }
}