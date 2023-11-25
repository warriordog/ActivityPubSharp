// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Conversion;
using ActivityPub.Types.Conversion.Converters;
using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Internal.Pivots;
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
        ASTypeInfoCache = new ASTypeInfoCache();
        ASTypeInfoCache.RegisterAllAssemblies();

        JsonLdSerializerOptions = new JsonLdSerializerOptions();
        var serializerOptions = Options.Create(JsonLdSerializerOptions);

        ConversionOptions = new ConversionOptions();
        var conversionOptions = Options.Create(ConversionOptions);
        var namelessEntityPivot = new NamelessEntityPivot();
        var anonymousEntityPivot = new AnonymousEntityPivot();
        var customConvertedEntityPivot = new CustomConvertedEntityPivot();
        var typeMapConverter = new TypeMapConverter(ASTypeInfoCache, conversionOptions, anonymousEntityPivot, namelessEntityPivot, customConvertedEntityPivot);

        var asTypeConverter = new ASTypeConverter();
        var linkableConverter = new LinkableConverter(ASTypeInfoCache);
        var listableConverter = new ListableConverter();
        var listableReadOnlyConverter = new ListableReadOnlyConverter();
        JsonLdSerializer = new JsonLdSerializer(serializerOptions, typeMapConverter, asTypeConverter, linkableConverter, listableConverter, listableReadOnlyConverter);
    }

    internal IASTypeInfoCache ASTypeInfoCache { get; }
    internal IJsonLdSerializer JsonLdSerializer { get; }
    internal JsonLdSerializerOptions JsonLdSerializerOptions { get; }
    internal ConversionOptions ConversionOptions { get; }
}