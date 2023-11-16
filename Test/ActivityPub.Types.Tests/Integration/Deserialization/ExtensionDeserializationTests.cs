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

        [Fact]
        public void Convert_FromEntitySelector()
        {
            SerializerFixture.ConversionOptions.AnonymousEntitySelectors.Add
            (
                new AnonymousEntitySelectorFake
                {
                    PropertyNameMapping =
                    {
                        [nameof(ASActivity.Actor)] = typeof(ASActivityEntity)
                    }
                }    
            );
            JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object","Actor":{}}""";
            ObjectUnderTest.Is<ASActivity>().Should().BeTrue();
        }
        
        public AnonymousExtensionsShould(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    protected ExtensionDeserializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}
}