// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.AS.Extended.Object;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public abstract class UnknownObjectDeserializationTests : DeserializationTests<ASObject>
{
    private UnknownObjectDeserializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}

    public class ObjectWithKnownAndUnknownTypeShould(JsonLdSerializerFixture fixture)
        : UnknownObjectDeserializationTests(fixture)
    {

        [Fact]
        public void DeserializeToKnownType()
        {
            JsonUnderTest = """{"type":["MadeUpFakeType","Note"]}""";
            ObjectUnderTest.Is<NoteObject>().Should().BeTrue();
        }
    }

    public class ObjectWithOnlyUnknownTypeShould(JsonLdSerializerFixture fixture)
        : UnknownObjectDeserializationTests(fixture)
    {

        [Fact] // Note: this behavior is temporary and will change once we support extensions.
        public void DeserializeToObject()
        {
            JsonUnderTest = """{"type":"MadeUpFakeType"}""";
            ObjectUnderTest.Is<ASObject>().Should().BeTrue();
        }
    }

    public class ObjectWithUnknownPropertiesShould(JsonLdSerializerFixture fixture)
        : UnknownObjectDeserializationTests(fixture)
    {

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
            ObjectUnderTest.TypeMap.UnmappedProperties.Should().ContainKey("fake_made_up_property");
            ObjectUnderTest.TypeMap.UnmappedProperties!["fake_made_up_property"].GetString().Should().Be("made_up_value");
        }
    }
}