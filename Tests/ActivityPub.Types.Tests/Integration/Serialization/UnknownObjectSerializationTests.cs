// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public abstract class UnknownObjectSerializationTests : SerializationTests
{
    public class ObjectWithUnknownTypeShould : UnknownObjectSerializationTests
    {
        private List<string>? TypesUnderTest { get; }
        
        public ObjectWithUnknownTypeShould(JsonLdSerializerFixture fixture) : base(fixture)
        {
            ObjectUnderTest = new ASObject
            {
                Types = new()
                {
                    "UnknownType1",
                    ASObject.ObjectType,
                    "UnknownType2"
                }
            };
                
            TypesUnderTest = JsonUnderTest.GetProperty("type").Deserialize<List<string>>();
        }

        [Fact]
        public void WriteKnownType()
        {
            TypesUnderTest.Should().Contain(ASObject.ObjectType);
        }

        [Fact]
        public void WriteUnknownTypes()
        {
            TypesUnderTest.Should().Contain("UnknownType1");
            TypesUnderTest.Should().Contain("UnknownType2");
        }
    }

    public class ObjectWithUnknownPropertiesShould : UnknownObjectSerializationTests
    {
        private const string UnknownPropName = "special_unknown_property";
        private const string UnknownPropValue = "special_unknown_value";
        
        public ObjectWithUnknownPropertiesShould(JsonLdSerializerFixture fixture) : base(fixture)
        {
            ObjectUnderTest = new ASObject
            {
                Duration = "PTS5",
                UnknownJsonProperties =
                {
                    [UnknownPropName] = JsonSerializer.SerializeToElement(UnknownPropValue)
                }
            };
        }
        
        [Fact]
        public void WriteKnownProperties()
        {
            JsonUnderTest.Should().HaveStringProperty("duration", "PTS5");
        }

        [Fact]
        public void WriteUnknownProperties()
        {
            JsonUnderTest.Should().HaveStringProperty(UnknownPropName, UnknownPropValue);
        }
    }

    private UnknownObjectSerializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}
}