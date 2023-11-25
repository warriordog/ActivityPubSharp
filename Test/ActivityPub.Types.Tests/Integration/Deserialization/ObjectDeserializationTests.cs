// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public abstract class ObjectDeserializationTests : DeserializationTests<ASObject>
{
    public class ContentPropertyShould : ObjectDeserializationTests
    {
        [Fact]
        public void ReadFromContentMapJson()
        {
            JsonUnderTest = """{"content":"default","contentMap":{"en":"english","en-us":"english US","es":"spanish"}}""";
        
            ObjectUnderTest.Content.Should().NotBeNull();
            ObjectUnderTest.Content!["en"].Should().Be("english");
            ObjectUnderTest.Content!["en", "us"].Should().Be("english US");
            ObjectUnderTest.Content!["es"].Should().Be("spanish");
        }

        [Fact]
        public void ReadDefaultValueFromContentJson()
        {
            JsonUnderTest = """{"content":"default","contentMap":{"en":"english","en-us":"english US","es":"spanish"}}""";

            ObjectUnderTest.Content.Should().NotBeNull();
            ObjectUnderTest.Content!.DefaultValue.Should().Be("default");
        }

        [Fact]
        public void ReadFromContentMapJson_WithoutContentJson()
        {
            JsonUnderTest = """{"contentMap":{"en":"english","en-us":"english US","es":"spanish"}}""";
        
            ObjectUnderTest.Content.Should().NotBeNull();
            ObjectUnderTest.Content!["en"].Should().Be("english");
            ObjectUnderTest.Content!["en", "us"].Should().Be("english US");
            ObjectUnderTest.Content!["es"].Should().Be("spanish");
        }

        [Fact]
        public void ReadFromContentJson_WithoutContentMapJson()
        {
            JsonUnderTest = """{"content":"default"}""";

            ObjectUnderTest.Content.Should().NotBeNull();
            ObjectUnderTest.Content!.DefaultValue.Should().Be("default");
        }

        public ContentPropertyShould(JsonLdSerializerFixture fixture) : base(fixture) {}
    }
    
    protected ObjectDeserializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}
}