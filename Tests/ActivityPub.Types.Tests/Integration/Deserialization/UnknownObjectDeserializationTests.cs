// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public abstract class UnknownObjectDeserializationTests : DeserializationTests<ASObject>
{
    private UnknownObjectDeserializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}

    public class ObjectWithKnownAndUnknownTypeShould : UnknownObjectDeserializationTests
    {
        public ObjectWithKnownAndUnknownTypeShould(JsonLdSerializerFixture fixture) : base(fixture) {}

        [Fact] // Note: this behavior is temporary and will change once we support extensions.
        public void DeserializeToObjectOrDeclaredType()
        {
            JsonUnderTest = """{"type":["MadeUpFakeType","Person"]}""";
            ObjectUnderTest.Should().BeOfType<ASObject>();
        }
    }

    public class ObjectWithOnlyUnknownTypeShould : UnknownObjectDeserializationTests
    {
        public ObjectWithOnlyUnknownTypeShould(JsonLdSerializerFixture fixture) : base(fixture) {}

        [Fact] // Note: this behavior is temporary and will change once we support extensions.
        public void DeserializeToObjectOrDeclaredType()
        {
            JsonUnderTest = """{"type":"MadeUpFakeType"}""";
            ObjectUnderTest.Should().BeOfType<ASObject>();
        }
    }

    public class ObjectWithUnknownPropertiesShould : UnknownObjectDeserializationTests
    {
        public ObjectWithUnknownPropertiesShould(JsonLdSerializerFixture fixture) : base(fixture) {}

        [Fact]
        public void DeserializeKnownProperties()
        {
            JsonUnderTest = """{"type":"Object","duration":"PTS5"}""";
            ObjectUnderTest.Duration.Should().Be("PTS5");
        }

        [Fact]
        public void CaptureUnknownProperties()
        {
            JsonUnderTest = """{"type":"Object","fake_made_up_property":"made_up_value"}""";
            ObjectUnderTest.UnknownJsonProperties.Should().ContainKey("fake_made_up_property");
            ObjectUnderTest.UnknownJsonProperties["fake_made_up_property"].GetString().Should().Be("made_up_value");
        }
    }
}