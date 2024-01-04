// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public class ComplexObjectDeserializationTests(JsonLdSerializerFixture fixture) : DeserializationTests<ASObject>(fixture)
{
    [Fact]
    public void NestedObjectsShouldHaveLinkedContexts()
    {
        JsonUnderTest =
            """
            {
                "@context": "https://www.w3.org/ns/activitystreams",
                "audience": {
                    "@context": "https://example.com/audience.jsonld"
                },
                "attachment": {
                    "@context": "https://example.com/attachment.jsonld"
                }
            }
            """;

        var objectContext = ObjectUnderTest.JsonLDContext;
        objectContext.Parent.Should().BeNull();
        objectContext.Should().ContainSingle("https://www.w3.org/ns/activitystreams");
        
        var audienceContext = ObjectUnderTest.Audience.Single().Value!.JsonLDContext;
        audienceContext.Parent.Should().BeSameAs(objectContext);
        audienceContext.LocalContexts.Should().ContainSingle("https://example.com/audience.jsonld");
        
        var attachmentContext = ObjectUnderTest.Attachment.Single().Value!.JsonLDContext;
        attachmentContext.Parent.Should().BeSameAs(objectContext);
        attachmentContext.LocalContexts.Should().ContainSingle("https://example.com/attachment.jsonld");
    }    
}
