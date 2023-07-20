// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Tests.Integration.Serialization;

public class LinkSerializationTests : SerializationTests
{
    public class ASLinkShould : LinkSerializationTests
    {

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
            JsonUnderTest.Should().HaveProperty("mediaType");
            JsonUnderTest.GetProperty("mediaType").Should().BeJsonString(MediaType);
        }


        [Fact]
        public void SerializeToObject_WhenUnknownPropsAreSet()
        {
            const string UnknownProp = "some_unknown_prop";
            ObjectUnderTest = new ASLink
            {
                HRef = "https://example.com/",
                UnknownJsonProperties =
                {
                    [UnknownProp] = JsonSerializer.SerializeToElement(1)
                }
            };
            JsonUnderTest.ValueKind.Should().Be(JsonValueKind.Object);
            JsonUnderTest.Should().HaveProperty(UnknownProp);
        }
    }
}