// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Conversion;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public abstract class SerializationTests(JsonLdSerializerFixture fixture)
    : IClassFixture<JsonLdSerializerFixture>
{
    private Lazy<JsonElement> _jsonUnderTest = new(() => throw new ApplicationException("Test error: please set ObjectUnderTest before calling JsonUnderTest"));

    protected IJsonLdSerializer JsonLdSerializer { get; } = fixture.JsonLdSerializer;

    // This is cached for performance
    protected JsonElement JsonUnderTest => _jsonUnderTest.Value;

    protected ASType ObjectUnderTest
    {
        set => _jsonUnderTest = new Lazy<JsonElement>(() => JsonLdSerializer.SerializeToElement(value));
    }
}