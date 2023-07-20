// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Nodes;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public class UnknownObjectSerializationTests : SerializationTests
{
    public class ObjectWithUnknownTypeShould : UnknownObjectSerializationTests
    {
        private List<string>? TypesUnderTest { get; }
        
        public ObjectWithUnknownTypeShould()
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
        
        public ObjectWithUnknownPropertiesShould()
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
            JsonUnderTest.GetProperty("duration").GetString().Should().Be("PTS5");
        }

        [Fact]
        public void WriteUnknownProperties()
        {
            JsonUnderTest.GetProperty(UnknownPropName).GetString().Should().Be(UnknownPropValue);
        }
    }
}