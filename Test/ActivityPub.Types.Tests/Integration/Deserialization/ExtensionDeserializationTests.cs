// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Tests.Util.Fakes;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public abstract class ExtensionDeserializationTests : DeserializationTests<ASObject>
{
    public class AnonymousExtensionsShould : ExtensionDeserializationTests
    {
        [Fact]
        public void NotConvert_FromNormalObject()
        {
            JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object"}""";
            ObjectUnderTest.Is<AnonymousExtensionFake>().Should().BeFalse();
        }

        [Fact]
        public void Convert_FromStringProperty()
        {
            JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object","ExtendedString":"Hello, world!"}""";
            
            ObjectUnderTest.Is<AnonymousExtensionFake>().Should().BeTrue();
            ObjectUnderTest.As<AnonymousExtensionFake>().ExtendedString.Should().Be("Hello, world!");
        }

        [Fact]
        public void Convert_FromIntProperty()
        {
            JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object","ExtendedInt":123}""";
            
            ObjectUnderTest.Is<AnonymousExtensionFake>().Should().BeTrue();
            ObjectUnderTest.As<AnonymousExtensionFake>().ExtendedInt.Should().Be(123);
        }

        [Fact]
        public void Convert_FromBothProperties()
        {
            JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object","ExtendedString":"Hello, world!","ExtendedInt":123}""";
            
            ObjectUnderTest.Is<AnonymousExtensionFake>().Should().BeTrue();
            ObjectUnderTest.As<AnonymousExtensionFake>().ExtendedString.Should().Be("Hello, world!");
            ObjectUnderTest.As<AnonymousExtensionFake>().ExtendedInt.Should().Be(123);
        }
        
        public AnonymousExtensionsShould(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    public class NamelessExtensionsShould : ExtensionDeserializationTests
    {

        [Fact]
        public void NotConvert_FromNormalObject()
        {
            JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object","ExtendedString":"Hello, world!"}""";
            ObjectUnderTest.Is<NamelessExtensionFake>().Should().BeFalse();
        }
        
        [Fact]
        public void Convert_FromMatchingContext()
        {            
            JsonUnderTest = """{"@context":["https://www.w3.org/ns/activitystreams","https://example.com/nameless"],"type":"Object","ExtendedString":"Hello, world!"}""";
            
            ObjectUnderTest.Is<AnonymousExtensionFake>().Should().BeTrue();
            ObjectUnderTest.As<AnonymousExtensionFake>().ExtendedString.Should().Be("Hello, world!");
        }
        
        public NamelessExtensionsShould(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    public class APActorShould : ExtensionDeserializationTests
    {
        
        [Fact]
        public void NotConvert_FromNormalObject()
        {
            JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object"}""";
            ObjectUnderTest.Is<APActor>().Should().BeFalse();
        }
        
        [Fact]
        public void NotConvert_FromInboxOnly()
        {
            JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object","inbox":"https://example.com/inbox"}""";
            ObjectUnderTest.Is<APActor>().Should().BeFalse();
        }
        
        [Fact]
        public void NotConvert_FromOutboxOnly()
        {
            JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object","outbox":"https://example.com/outbox"}""";
            ObjectUnderTest.Is<APActor>().Should().BeFalse(); 
        }
        
        [Fact]
        public void Convert_FromInboxAndOutbox()
        {
            JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object","inbox":"https://example.com/inbox","outbox":"https://example.com/outbox"}""";
            
            ObjectUnderTest.Is<APActor>().Should().BeTrue();
            ObjectUnderTest.As<APActor>().Inbox.HRef.Should().Be("https://example.com/inbox");
            ObjectUnderTest.As<APActor>().Outbox.HRef.Should().Be("https://example.com/outbox");
        }
        
        public APActorShould(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    protected ExtensionDeserializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}
}