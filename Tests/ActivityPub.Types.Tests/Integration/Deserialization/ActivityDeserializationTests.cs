// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public abstract class ActivityDeserializationTests : DeserializationTests<ASActivity>
{
    [Fact]
    public void BasicActivitiesShould_DeserializeToASActivity()
    {
        JsonUnderTest = """{"type":"Activity"}""";
        ObjectUnderTest.Should().BeOfType<ASActivity>();
    }
    
    [Fact]
    public void TransitiveActivitiesShould_DeserializeToASTransitiveActivity()
    {
        JsonUnderTest = """{"type":"Activity","object":{}}""";
        ObjectUnderTest.Should().BeOfType<ASTransitiveActivity>();
    }
    
    [Fact]
    public void TargetedActivitiesShould_DeserializeToASTargetedActivity()
    {
        JsonUnderTest = """{"type":"Activity","object":{},"target":{}}""";
        ObjectUnderTest.Should().BeOfType<ASTargetedActivity>();
    }
}