// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Internal.TypeInfo;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public abstract class SerializationTests
{
    private readonly IJsonLdSerializer _jsonLdSerializer;
    
    protected SerializationTests()
    {
        var jsonTypeInfoCache = new JsonTypeInfoCache();
        var asTypeInfoCache = new ASTypeInfoCache(jsonTypeInfoCache);
        asTypeInfoCache.RegisterAllAssemblies();
        
        _jsonLdSerializer = new JsonLdSerializer(asTypeInfoCache, jsonTypeInfoCache);
        _jsonUnderTest = new Lazy<JsonElement>(() => _jsonLdSerializer.SerializeToElement(ObjectUnderTest));
    }

    // This is cached for performance
    protected JsonElement JsonUnderTest => _jsonUnderTest.Value;
    private Lazy<JsonElement> _jsonUnderTest;

    protected ASObject ObjectUnderTest
    {
        get => _objectUnderTest;
        set
        {
            _objectUnderTest = value;

            // TODO maybe we could create a ResettableLazy, then this wouldn't need to be duplicated
            _jsonUnderTest = new Lazy<JsonElement>(() => _jsonLdSerializer.SerializeToElement(ObjectUnderTest));
        }
    }
    private ASObject _objectUnderTest = new();
}