// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Conversion;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public abstract class DeserializationTests<T>(JsonLdSerializerFixture fixture)
    : IClassFixture<JsonLdSerializerFixture>
{
    // Cached for performance
    private Lazy<T> _objectUnderTest = new(() => throw new ApplicationException("Test error: please set JsonUnderTest before calling ObjectUnderTest"));

    protected JsonLdSerializerFixture SerializerFixture { get; } = fixture;
    protected IJsonLdSerializer JsonLdSerializer => SerializerFixture.JsonLdSerializer;

    protected string JsonUnderTest
    {
        set => _objectUnderTest = new Lazy<T>(() => JsonLdSerializer.Deserialize<T>(value) ?? throw new ApplicationException("Deserialization failed!"));
    }

    protected T ObjectUnderTest => _objectUnderTest.Value;
}