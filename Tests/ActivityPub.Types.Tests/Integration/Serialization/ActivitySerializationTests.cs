// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public class ActivitySerializationTests : SerializationTests
{
    [Fact]
    public void BasicActivitiesShould_NotIncludeObject()
    {
        ObjectUnderTest = new ASActivity();
        JsonUnderTest.Should().NotHaveProperty("object");
    }

    [Fact]
    public void BasicActivitiesShould_NotIncludeTarget()
    {
        ObjectUnderTest = new ASActivity();
        JsonUnderTest.Should().NotHaveProperty("target");
    }
    
    [Fact]
    public void TransitiveActivitiesShould_IncludeObject()
    {
        ObjectUnderTest = new ASTransitiveActivity
        {
            Object = new ASObject()
        };
        JsonUnderTest.Should().HaveProperty("object");
    }
    
    [Fact]
    public void TransitiveActivitiesShould_NotIncludeTarget()
    {
        ObjectUnderTest = new ASTransitiveActivity
        {
            Object = new ASObject()
        };
        JsonUnderTest.Should().NotHaveProperty("target");
    }
    
    [Fact]
    public void TargetedActivitiesShould_IncludeObjectAndTarget()
    {
        ObjectUnderTest = new ASTransitiveActivity
        {
            Object = new ASObject(),
            Target = new ASObject()
        };
        JsonUnderTest.Should().HaveProperty("object");
        JsonUnderTest.Should().HaveProperty("target");
    }
    
    public ActivitySerializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}
}