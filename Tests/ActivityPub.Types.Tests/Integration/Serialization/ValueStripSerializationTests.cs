// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Collection;
using ActivityPub.Types.Extended.Object;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public class ValueStripSerializationTests : SerializationTests
{
    [Fact]
    public void NullObjectsShould_BeStrippedFromOutput()
    {
        ObjectUnderTest = new ASObject
        {
            Image = null
        };

        JsonUnderTest.Should().NotHaveProperty("image");
    }

    [Fact]
    public void DefaultValuesShould_BeStrippedFromOutput()
    {
        ObjectUnderTest = new ASObject
        {
            StartTime = default(DateTime)
        };
        
        JsonUnderTest.Should().NotHaveProperty("startTime");
    }

    [Fact]
    public void EmptyCollectionsShould_BeStrippedFromOutput()
    {
        ObjectUnderTest = new ASObject
        {
            Attachment = new()
        };
            
        JsonUnderTest.Should().NotHaveProperty("attachment");
    }

    [Fact]
    public void NullCollectionsShould_BeStrippedFromOutput()
    {
        ObjectUnderTest = new ASCollection<ASObject>
        {
            Items = null
        };
            
        JsonUnderTest.Should().NotHaveProperty("items");
    }

    [Fact]
    public void NonNullObjectsShould_BePreserved()
    {
        ObjectUnderTest = new ASObject
        {
            Image = new ImageObject()
        };

        JsonUnderTest.Should().HaveProperty("image");
    }

    [Fact]
    public void NonDefaultValuesShould_BePreserved()
    {
        ObjectUnderTest = new ASObject
        {
            StartTime = DateTime.Now
        };
        
        JsonUnderTest.Should().HaveProperty("startTime");
    }

    [Fact]
    public void NonEmptyCollectionsShould_BePreserved()
    {
        ObjectUnderTest = new ASObject
        {
            Attachment = new()
            {
                new ASObject()                
            }
        };
            
        JsonUnderTest.Should().HaveProperty("attachment");
    }
    
    public ValueStripSerializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}
}