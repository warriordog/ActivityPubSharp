// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Internal.TypeInfo;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public abstract class DeserializationTests
{
    private readonly IJsonLdSerializer _jsonLdSerializer;
    
    protected DeserializationTests()
    {
        // TODO remove this once we support DI in tests
        var jsonTypeInfoCache = new JsonTypeInfoCache();
        var asTypeInfoCache = new ASTypeInfoCache(jsonTypeInfoCache);
        asTypeInfoCache.RegisterAllAssemblies();
        
        _jsonLdSerializer = new JsonLdSerializer(asTypeInfoCache, jsonTypeInfoCache);
        _objectUnderTest = new Lazy<ASObject>(() => _jsonLdSerializer.Deserialize<ASObject>(JsonUnderTest) ?? throw new ApplicationException("Deserialization failed!"));
    }

    protected string JsonUnderTest
    {
        get => _jsonUnderTest;
        set
        {
            _jsonUnderTest = value;
            _objectUnderTest = new Lazy<ASObject>(() => _jsonLdSerializer.Deserialize<ASObject>(JsonUnderTest) ?? throw new ApplicationException("Deserialization failed!"));
        }
    }
    private string _jsonUnderTest = "null";

    // Cached for performance
    private Lazy<ASObject> _objectUnderTest;
    protected ASObject ObjectUnderTest => _objectUnderTest.Value;
}