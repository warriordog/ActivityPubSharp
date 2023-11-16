// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Conversion;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public abstract class DeserializationTests<T> : IClassFixture<JsonLdSerializerFixture>
{
    // Cached for performance
    private Lazy<T> _objectUnderTest;

    protected JsonLdSerializerFixture SerializerFixture { get; }
    protected IJsonLdSerializer JsonLdSerializer => SerializerFixture.JsonLdSerializer;
    
    protected DeserializationTests(JsonLdSerializerFixture fixture)
    {
        SerializerFixture = fixture;
        _objectUnderTest = new Lazy<T>(() => throw new ApplicationException("Test error: please set JsonUnderTest before calling ObjectUnderTest"));
    }

    protected string JsonUnderTest
    {
        set => _objectUnderTest = new Lazy<T>(() => JsonLdSerializer.Deserialize<T>(value) ?? throw new ApplicationException("Deserialization failed!"));
    }

    protected T ObjectUnderTest => _objectUnderTest.Value;
}