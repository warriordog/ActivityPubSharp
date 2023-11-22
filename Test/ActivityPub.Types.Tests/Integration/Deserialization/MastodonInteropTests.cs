// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public class MastodonInteropTests : DeserializationTests<ASObject>
{
    public MastodonInteropTests(JsonLdSerializerFixture fixture) : base(fixture) { }

    [Fact]
    public void MastodonFollowActivityTest()
    {
        JsonUnderTest = """
                        {
                          "@context": "https://www.w3.org/ns/activitystreams",
                          "id": "http://mastodon.example/users/user#follows/32/undo",
                          "type": "Undo",
                          "actor": "http://mastodon.example/users/user",
                          "object": {
                            "id": "http://mastodon.example/6dc45337-040b-43d2-8238-de9b7f377750",
                            "type": "Follow",
                            "actor": "http://mastodon.example/users/user",
                            "object": "https://host.example/actor/ahwPS_-DYE2h9RlTo0p4jg"
                          }
                        }
                        """;
        
        ObjectUnderTest.Is<ASActivity>(out var activity).Should().BeTrue();
        activity!.Actor.Should().NotBeEmpty();
        activity!.Object.Should().NotBeEmpty();
        
        activity.Object!.First()!.Value!.Is<ASActivity>(out var nestedActivity).Should().BeTrue();
        nestedActivity!.Actor.Should().NotBeEmpty();
        nestedActivity!.Object.Should().NotBeEmpty();
    }
    
    [Fact]
    public void MastodonAcceptFollowActivityTest()
    {
        JsonUnderTest = """
                        {
                          "@context": "https://www.w3.org/ns/activitystreams",
                          "type": "Accept",
                          "actor": "https://host.example/actor/ahwPS_-DYE2h9RlTo0p4jg",
                          "object": {
                            "id": "http://mastodon.example/fbdfbd1c-489e-4edb-980f-0273a03c0992",
                            "type": "Follow",
                            "actor": "http://mastodon.example/users/user"
                          }
                        }
                        """;
        
        ObjectUnderTest.Is<ASActivity>(out var activity).Should().BeTrue();
        activity!.Actor.Should().NotBeEmpty();
        activity!.Object.Should().NotBeEmpty();
        
        activity.Object!.First()!.Value!.Is<ASActivity>(out var nestedActivity).Should().BeTrue();
        nestedActivity!.Actor.Should().NotBeEmpty();
    }
    
    [Fact(Skip = "ContentMap not deserializing")]
    public void MastodonUpateNoteActivityTest()
    {
        JsonUnderTest = """
                        {
                          "@context": [
                            "https://www.w3.org/ns/activitystreams",
                            {
                              "atomUri": "ostatus:atomUri",
                              "conversation": "ostatus:conversation",
                              "inReplyToAtomUri": "ostatus:inReplyToAtomUri",
                              "ostatus": "http://ostatus.org#",
                              "sensitive": "as:sensitive",
                              "toot": "http://joinmastodon.org/ns#",
                              "votersCount": "toot:votersCount"
                            }
                          ],
                          "actor": "https://mastodon.example/users/john",
                          "cc": ["https://mastodon.example/users/john/followers"],
                          "id": "https://mastodon.example/users/john/statuses/9876#updates/1675500855",
                          "object": {
                            "atomUri": "https://mastodon.example/users/john/statuses/9876",
                            "attachment": [],
                            "attributedTo": "https://mastodon.example/users/john",
                            "cc": ["https://mastodon.example/users/john/followers"],
                            "content": "<p>Creating test data 🆕</p>",
                            "contentMap": {
                              "en": "<p>Creating test data 🆕</p>",
                              "es": "<p>Creando datos de prueba 🆕</p>"
                            },
                            "conversation": "tag:mas.to,2023-02-04:objectId=8765:objectType=Conversation",
                            "id": "https://mastodon.example/users/john/statuses/9876",
                            "inReplyTo": null,
                            "inReplyToAtomUri": null,
                            "published": "2023-02-04T08:52:55Z",
                            "replies": {
                              "first": {
                                "items": [],
                                "next": "https://mastodon.example/users/john/statuses/9876/replies?only_other_accounts=true&page=true",
                                "partOf": "https://mastodon.example/users/john/statuses/9876/replies",
                                "type": "CollectionPage"
                              },
                              "id": "https://mastodon.example/users/john/statuses/9876/replies",
                              "type": "Collection"
                            },
                            "sensitive": false,
                            "summary": null,
                            "tag": [],
                            "to": ["https://www.w3.org/ns/activitystreams#Public"],
                            "type": "Note",
                            "updated": "2023-02-04T08:54:15Z",
                            "url": "https://mastodon.example/@john/9876"
                          },
                          "published": "2023-02-04T08:54:15Z",
                          "signature": {
                            "created": "2023-02-04T08:54:15Z",
                            "creator": "https://mastodon.example/users/john#main-key",
                            "signatureValue": "fake==",
                            "type": "RsaSignature2017"
                          },
                          "to": ["https://www.w3.org/ns/activitystreams#Public"],
                          "type": "Update"
                        }
                        """;
        
        ObjectUnderTest.Is<ASActivity>(out var activity).Should().BeTrue();
        activity!.Object.Should().NotBeEmpty();
        
        activity.Object!.First()!.Value!.Is<ASObject>(out var note).Should().BeTrue();
        Assert.NotNull(note);
        note.Summary.Should().BeNull();
        // The language maps seem not to be working. Or, they're not mapped correctly.
        note.Content!.SingleString.Should().Be("<p>Creating test data \ud83c\udd95</p>");
        // Fails. But passes if I add and use a ContentMap property
        note.Content!.GetOrNull("es").Should().Be("<p>Creando datos de prueba \ud83c\udd95</p>");
        note.Content!.GetOrNull("en").Should().Be("<p>Creating test data \ud83c\udd95</p>");
        // Fails no matter what. Maybe that's by design? But I would expect it to return null.
        note.Content!.GetOrNull("de").Should().BeNull();
        

        note.Replies.Should().BeEmpty();
        note.Replies!.Id.Should().Be("https://mastodon.example/users/john/statuses/9876/replies");
    }
    
    [Fact]
    public void MastodonActor()
    {
        JsonUnderTest = """
                        {
                          "@context": [
                            "https://www.w3.org/ns/activitystreams",
                            "https://w3id.org/security/v1",
                            {
                              "manuallyApprovesFollowers": "as:manuallyApprovesFollowers",
                              "featured": {
                                "@id": "toot:featured",
                                "@type": "@id"
                              },
                              "toot": "http://joinmastodon.org/ns#",
                              "featuredTags": {
                                "@id": "toot:featuredTags",
                                "@type": "@id"
                              },
                              "alsoKnownAs": {
                                "@id": "as:alsoKnownAs",
                                "@type": "@id"
                              },
                              "movedTo": {
                                "@id": "as:movedTo",
                                "@type": "@id"
                              },
                              "schema": "http://schema.org#",
                              "PropertyValue": "schema:PropertyValue",
                              "value": "schema:value",
                              "discoverable": "toot:discoverable",
                              "Device": "toot:Device",
                              "Ed25519Signature": "toot:Ed25519Signature",
                              "Ed25519Key": "toot:Ed25519Key",
                              "Curve25519Key": "toot:Curve25519Key",
                              "EncryptedMessage": "toot:EncryptedMessage",
                              "publicKeyBase64": "toot:publicKeyBase64",
                              "deviceId": "toot:deviceId",
                              "claim": {
                                "@type": "@id",
                                "@id": "toot:claim"
                              },
                              "fingerprintKey": {
                                "@type": "@id",
                                "@id": "toot:fingerprintKey"
                              },
                              "identityKey": {
                                "@type": "@id",
                                "@id": "toot:identityKey"
                              },
                              "devices": {
                                "@type": "@id",
                                "@id": "toot:devices"
                              },
                              "messageFranking": "toot:messageFranking",
                              "messageType": "toot:messageType",
                              "cipherText": "toot:cipherText",
                              "suspended": "toot:suspended",
                              "focalPoint": {
                                "@container": "@list",
                                "@id": "toot:focalPoint"
                              }
                            }
                          ],
                          "id": "https://mastodon.example/users/test_actor",
                          "type": "Person",
                          "following": "https://mastodon.example/users/test_actor/following",
                          "followers": "https://mastodon.example/users/test_actor/followers",
                          "inbox": "https://mastodon.example/users/test_actor/inbox",
                          "outbox": "https://mastodon.example/users/test_actor/outbox",
                          "featured": "https://mastodon.example/users/test_actor/collections/featured",
                          "featuredTags": "https://mastodon.example/users/test_actor/collections/tags",
                          "preferredUsername": "test_actor",
                          "name": "An actor you can test",
                          "summary": "<p>Not real.</p><p>Still helpful.</p>",
                          "url": "https://mastodon.example/@test_actor",
                          "manuallyApprovesFollowers": false,
                          "discoverable": true,
                          "published": "2022-12-12T00:00:00Z",
                          "devices": "https://mastodon.example/users/test_actor/collections/devices",
                          "publicKey": {
                            "id": "https://mastodon.example/users/test_actor#main-key",
                            "owner": "https://mastodon.example/users/test_actor",
                            "publicKeyPem": "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAo4uzFzVM3y8MCx7ocF59\nEVOTdHrnlxtdcob3giqqLUWT1cXEVoV4lFWiFO2Gystc6m+sVR8Sd5WU+M7OwRoo\nf+E0dAZk1bdIuFf9xsVRm4aJSidl8+u0guNgJrwIxmPCgi/Ji367L0pBR6JVLFeF\nFMX3zoH1a1+44vd5iJkxNyjP3nzY8+xCiOMqheTsxDmIctlDqeu0I4mZOJL8ebyo\nSYR8O/9unoQxepcTlG+CZsPURs9TcUyOX8oJkDlgA84zD4b3tkYa3fTCgJ+ByXqB\nWeJ2UShS4k6aVk0Z17tCqj0qGK9U3c/Dc+pKXdy6Ce2Yl2a1aHxnuTO4YH3CRaN4\nxQIDAQAB\n-----END PUBLIC KEY-----\n"
                          },
                          "tag": [],
                          "attachment": [
                            {
                              "type": "PropertyValue",
                              "name": "blog",
                              "value": "<a href=\"https://test_actor.blogspot.com\" target=\"_blank\" rel=\"nofollow noopener noreferrer me\"><span class=\"invisible\">https://</span><span class=\"\">test_actor.blogspot.com</span><span class=\"invisible\"></span></a>"
                            },
                            {
                              "type": "PropertyValue",
                              "name": "email",
                              "value": "test_actor@example.com"
                            }
                          ],
                          "endpoints": {
                            "sharedInbox": "https://mastodon.example/inbox"
                          },
                          "icon": {
                            "type": "Image",
                            "mediaType": "image/jpeg",
                            "url": "https://cdn.mastodon.example/test_actor/accounts/avatars/109/497/783/827/254/564/original/b0adb5063df194a6.jpg"
                          },
                          "image": {
                            "type": "Image",
                            "mediaType": "image/jpeg",
                            "url": "https://cdn.masto.host/test_actor/accounts/headers/109/497/783/827/254/564/original/b257a4a7bcbed48b.jpeg"
                          }
                        }
                        """;
        
        ObjectUnderTest.Is<APActor>(out var actor).Should().BeTrue();
        actor!.Endpoints!.Value!.SharedInbox.Should().NotBeNull();
        actor.Image.Should().NotBeNull();
        actor.Icon.Should().NotBeNull();
        actor.Summary.Should().NotBeNull();
        actor.Name.Should().NotBeNull();
    }
}