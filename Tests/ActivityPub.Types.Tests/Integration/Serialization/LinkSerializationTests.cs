// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public abstract class LinkSerializationTests : SerializationTests
{
    protected LinkSerializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}

    public class ASLinkShould : LinkSerializationTests
    {
        public ASLinkShould(JsonLdSerializerFixture fixture) : base(fixture) {}

        [Fact]
        public void SerializeToString_WhenOnlyHRefIsSet()
        {
            const string Url = "https://example.com/";
            ObjectUnderTest = new ASLink
            {
                HRef = Url
            };
            JsonUnderTest.Should().BeJsonString(Url);
        }


        [Fact]
        public void SerializeToObject_WhenOtherPropsAreSet()
        {
            const string MediaType = "text/html";
            ObjectUnderTest = new ASLink
            {
                HRef = "https://example.com/",
                MediaType = MediaType
            };
            JsonUnderTest.ValueKind.Should().Be(JsonValueKind.Object);
            JsonUnderTest.Should().HaveStringProperty("mediaType", MediaType);
        }


        [Fact]
        public void SerializeToObject_WhenUnknownPropsAreSet()
        {
            const string UnknownProp = "some_unknown_prop";
            var link = new ASLink
            {
                HRef = "https://example.com/",
                TypeMap =
                {
                    UnmappedProperties = new Dictionary<string, JsonElement>
                    {
                        [UnknownProp] = JsonSerializer.SerializeToElement(1)
                    }
                }
            };
            ObjectUnderTest = link;
            JsonUnderTest.ValueKind.Should().Be(JsonValueKind.Object);
            JsonUnderTest.Should().HaveProperty(UnknownProp);
        }
    }
}