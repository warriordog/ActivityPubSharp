// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Extended.Actor;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public abstract class ActorDeserializationTests : DeserializationTests<PersonActor>
{
    private ActorDeserializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}

    public class EndpointsShould : ActorDeserializationTests
    {
        public EndpointsShould(JsonLdSerializerFixture fixture) : base(fixture) {}

        [Fact]
        public void ConvertEndpoints()
        {
            JsonUnderTest = """
            {
                "@context": "https://www.w3.org/ns/activitystreams",
                "type": "Person",
                "inbox": "https://example.com/actor/inbox",
                "outbox": "https://example.com/actor/outbox",
                "image": {
                    "type": "Image"
                },
                "id": "https://example.com/actor/id",
                "endpoints": {
                    "proxyUrl": "https://example.com/proxyUrl",
                    "oauthAuthorizationEndpoint": "https://example.com/oauthAuthorizationEndpoint",
                    "oauthTokenEndpoint": "https://example.com/oauthTokenEndpoint",
                    "provideClientKey": "https://example.com/provideClientKey",
                    "signClientKey": "https://example.com/signClientKey",
                    "sharedInbox": "https://example.com/sharedInbox"
                }
            }
            """;

            ObjectUnderTest.Endpoints.Should().NotBeNull();
            ObjectUnderTest.Endpoints!.ProxyUrl?.HRef.Uri.ToString().Should().Be("https://example.com/proxyUrl");
            ObjectUnderTest.Endpoints!.OAuthAuthorizationEndpoint?.HRef.Uri.ToString().Should().Be("https://example.com/oauthAuthorizationEndpoint");
            ObjectUnderTest.Endpoints!.OAuthTokenEndpoint?.HRef.Uri.ToString().Should().Be("https://example.com/oauthTokenEndpoint");
            ObjectUnderTest.Endpoints!.ProvideClientKey?.HRef.Uri.ToString().Should().Be("https://example.com/provideClientKey");
            ObjectUnderTest.Endpoints!.SignClientKey?.HRef.Uri.ToString().Should().Be("https://example.com/signClientKey");
            ObjectUnderTest.Endpoints!.SharedInbox?.HRef.Uri.ToString().Should().Be("https://example.com/sharedInbox");
        }

        [Fact]
        public void IgnoreTypeInEndpoints()
        {
            JsonUnderTest = """
            {
                "@context": "https://www.w3.org/ns/activitystreams",
                "type": "Person",
                "inbox": "https://example.com/actor/inbox",
                "outbox": "https://example.com/actor/outbox",
                "endpoints": {
                    "type": "Object",
                    "proxyUrl": "https://example.com/proxyUrl"
                }
            }
            """;

            ObjectUnderTest.Endpoints.Should().NotBeNull();
        }
    }
}