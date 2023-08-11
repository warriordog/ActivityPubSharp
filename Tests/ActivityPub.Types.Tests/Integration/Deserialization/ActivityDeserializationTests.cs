// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public class ActivityDeserializationTests : DeserializationTests<ASActivity>
{
    public ActivityDeserializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}

    [Fact]
    public void BasicActivitiesShould_DeserializeToASActivity()
    {
        JsonUnderTest = """{"type":"Activity"}""";
        ObjectUnderTest.Is<ASActivity>().Should().BeTrue();
    }

    [Fact]
    public void TransitiveActivitiesShould_DeserializeToASTransitiveActivity()
    {
        JsonUnderTest = """{"type":"Activity","object":{}}""";
        ObjectUnderTest.Is<ASTransitiveActivity>().Should().BeTrue();
    }

    [Fact]
    public void TargetedActivitiesShould_DeserializeToASTargetedActivity()
    {
        JsonUnderTest = """{"type":"Activity","object":{},"target":{}}""";
        ObjectUnderTest.Is<ASTargetedActivity>().Should().BeTrue();
    }
}