// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Json;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public abstract class SerializationTests : IClassFixture<JsonLdSerializerFixture>
{
    private readonly IJsonLdSerializer _jsonLdSerializer;
    
    protected SerializationTests(JsonLdSerializerFixture fixture)
    {
        _jsonLdSerializer = fixture.JsonLdSerializer;
        _jsonUnderTest = new Lazy<JsonElement>(() => throw new ApplicationException("Test error: please set ObjectUnderTest before calling JsonUnderTest"));
    }

    // This is cached for performance
    protected JsonElement JsonUnderTest => _jsonUnderTest.Value;
    private Lazy<JsonElement> _jsonUnderTest;

    protected ASType ObjectUnderTest
    {
        set => _jsonUnderTest = new Lazy<JsonElement>(() => _jsonLdSerializer.SerializeToElement(value));
    }
}