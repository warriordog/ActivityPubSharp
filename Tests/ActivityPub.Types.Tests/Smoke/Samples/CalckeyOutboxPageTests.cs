// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Collection;
using ActivityPub.Types.Internal.TypeInfo;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Tests.Smoke.Samples;

public class CalckeyOutboxPageTests
{
    private const string PageJson =
        """
        {
          "@context": [
            "https://www.w3.org/ns/activitystreams",
            "https://w3id.org/security/v1",
            {
              "manuallyApprovesFollowers": "as:manuallyApprovesFollowers",
              "movedToUri": "as:movedTo",
              "sensitive": "as:sensitive",
              "Hashtag": "as:Hashtag",
              "quoteUri": "fedibird:quoteUri",
              "quoteUrl": "as:quoteUrl",
              "toot": "http://joinmastodon.org/ns#",
              "Emoji": "toot:Emoji",
              "featured": "toot:featured",
              "discoverable": "toot:discoverable",
              "schema": "http://schema.org#",
              "PropertyValue": "schema:PropertyValue",
              "value": "schema:value",
              "misskey": "https://misskey-hub.net/ns#",
              "_misskey_content": "misskey:_misskey_content",
              "_misskey_quote": "misskey:_misskey_quote",
              "_misskey_reaction": "misskey:_misskey_reaction",
              "_misskey_votes": "misskey:_misskey_votes",
              "_misskey_talk": "misskey:_misskey_talk",
              "isCat": "misskey:isCat",
              "fedibird": "http://fedibird.com/ns#",
              "vcard": "http://www.w3.org/2006/vcard/ns#"
            }
          ],
          "id": "https://enby.life/users/9fpnzspgtp/outbox?page=true",
          "partOf": "https://enby.life/users/9fpnzspgtp/outbox",
          "type": "OrderedCollectionPage",
          "totalItems": 5,
          "orderedItems": [
            {
              "id": "https://enby.life/notes/9h203ewltks9dbfb/activity",
              "actor": "https://enby.life/users/9fpnzspgtp",
              "type": "Create",
              "published": "2023-07-11T15:17:48.213Z",
              "object": {
                "id": "https://enby.life/notes/9h203ewltks9dbfb",
                "type": "Note",
                "attributedTo": "https://enby.life/users/9fpnzspgtp",
                "summary": null,
                "content": "<p><span>Source for the instance background: </span><a href=\"https://tech.lgbt/@Gingeh/110117861681527722\">https://tech.lgbt/@Gingeh/110117861681527722</a></p>",
                "_misskey_content": "Source for the instance background: https://tech.lgbt/@Gingeh/110117861681527722",
                "source": {
                  "content": "Source for the instance background: https://tech.lgbt/@Gingeh/110117861681527722",
                  "mediaType": "text/x.misskeymarkdown"
                },
                "published": "2023-07-11T15:17:48.213Z",
                "to": [
                  "https://www.w3.org/ns/activitystreams#Public"
                ],
                "cc": [
                  "https://enby.life/users/9fpnzspgtp/followers"
                ],
                "inReplyTo": null,
                "attachment": [],
                "sensitive": false,
                "tag": []
              },
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpnzspgtp/followers"
              ]
            },
            {
              "id": "https://enby.life/notes/9h202z6wvqu3unrm/activity",
              "actor": "https://enby.life/users/9fpnzspgtp",
              "type": "Create",
              "published": "2023-07-11T15:17:27.848Z",
              "object": {
                "id": "https://enby.life/notes/9h202z6wvqu3unrm",
                "type": "Note",
                "attributedTo": "https://enby.life/users/9fpnzspgtp",
                "summary": null,
                "content": "<p><span>Finally figured out how to set an instance-wide background image on </span><a href=\"https://enby.life/tags/Calckey\" rel=\"tag\">#Calckey</a><span>. I had to use custom CSS, but its looks really slick and even adapts to match each user's theme!</span></p>",
                "_misskey_content": "Finally figured out how to set an instance-wide background image on #Calckey. I had to use custom CSS, but its looks really slick and even adapts to match each user's theme!",
                "source": {
                  "content": "Finally figured out how to set an instance-wide background image on #Calckey. I had to use custom CSS, but its looks really slick and even adapts to match each user's theme!",
                  "mediaType": "text/x.misskeymarkdown"
                },
                "published": "2023-07-11T15:17:27.848Z",
                "to": [
                  "https://www.w3.org/ns/activitystreams#Public"
                ],
                "cc": [
                  "https://enby.life/users/9fpnzspgtp/followers"
                ],
                "inReplyTo": null,
                "attachment": [],
                "sensitive": false,
                "tag": [
                  {
                    "type": "Hashtag",
                    "href": "https://enby.life/tags/calckey",
                    "name": "#calckey"
                  }
                ]
              },
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpnzspgtp/followers"
              ]
            },
            {
              "id": "https://enby.life/notes/9gjh1o24lu3c00f3/activity",
              "actor": "https://enby.life/users/9fpnzspgtp",
              "type": "Create",
              "published": "2023-06-28T16:04:42.892Z",
              "object": {
                "id": "https://enby.life/notes/9gjh1o24lu3c00f3",
                "type": "Note",
                "attributedTo": "https://enby.life/users/9fpnzspgtp",
                "summary": null,
                "content": "<p><a href=\"https://enby.life/@hazelnoot\" class=\"u-url mention\">@hazelnoot</a><span> crisis averted - I've regained the admin account. Nothing that a bit of root access couldn't fix!</span></p>",
                "_misskey_content": "@hazelnoot crisis averted - I've regained the admin account. Nothing that a bit of root access couldn't fix!",
                "source": {
                  "content": "@hazelnoot crisis averted - I've regained the admin account. Nothing that a bit of root access couldn't fix!",
                  "mediaType": "text/x.misskeymarkdown"
                },
                "published": "2023-06-28T16:04:42.892Z",
                "to": [
                  "https://www.w3.org/ns/activitystreams#Public"
                ],
                "cc": [
                  "https://enby.life/users/9fpnzspgtp/followers"
                ],
                "inReplyTo": "https://enby.life/notes/9gje2bq6sq0k83t1",
                "attachment": [],
                "sensitive": false,
                "tag": [
                  {
                    "type": "Mention",
                    "href": "https://enby.life/users/9fpwmts9tv",
                    "name": "@hazelnoot"
                  }
                ]
              },
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpnzspgtp/followers"
              ]
            },
            {
              "id": "https://enby.life/notes/9fpx4niku4/activity",
              "actor": "https://enby.life/users/9fpnzspgtp",
              "type": "Create",
              "published": "2023-06-07T23:41:50.732Z",
              "object": {
                "id": "https://enby.life/notes/9fpx4niku4",
                "type": "Note",
                "attributedTo": "https://enby.life/users/9fpnzspgtp",
                "summary": null,
                "content": "<p><span>testing testing </span>🦊</p>",
                "_misskey_content": "testing testing 🦊",
                "source": {
                  "content": "testing testing 🦊",
                  "mediaType": "text/x.misskeymarkdown"
                },
                "published": "2023-06-07T23:41:50.732Z",
                "to": [
                  "https://www.w3.org/ns/activitystreams#Public"
                ],
                "cc": [
                  "https://enby.life/users/9fpnzspgtp/followers"
                ],
                "inReplyTo": null,
                "attachment": [],
                "sensitive": false,
                "tag": []
              },
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpnzspgtp/followers"
              ]
            },
            {
              "id": "https://enby.life/notes/9fpo5w7str/activity",
              "actor": "https://enby.life/users/9fpnzspgtp",
              "type": "Create",
              "published": "2023-06-07T19:30:52.120Z",
              "object": {
                "id": "https://enby.life/notes/9fpo5w7str",
                "type": "Note",
                "attributedTo": "https://enby.life/users/9fpnzspgtp",
                "summary": null,
                "content": "<p><span>test post</span></p>",
                "_misskey_content": "test post",
                "source": {
                  "content": "test post",
                  "mediaType": "text/x.misskeymarkdown"
                },
                "published": "2023-06-07T19:30:52.120Z",
                "to": [
                  "https://www.w3.org/ns/activitystreams#Public"
                ],
                "cc": [
                  "https://enby.life/users/9fpnzspgtp/followers"
                ],
                "inReplyTo": null,
                "attachment": [],
                "sensitive": false,
                "tag": []
              },
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpnzspgtp/followers"
              ]
            }
          ],
          "prev": "https://enby.life/users/9fpnzspgtp/outbox?page=true&since_id=9h203ewltks9dbfb",
          "next": "https://enby.life/users/9fpnzspgtp/outbox?page=true&until_id=9fpo5w7str"
        }
        """;

    [Fact]
    public void ShouldDeserializeWithoutCrash()
    {
        var result = _jsonLdSerializer.Deserialize<ASType>(PageJson);
        result.Should().NotBeNull();
    }

    [Fact]
    public void ShouldSetKeyFields()
    {
        var result = _jsonLdSerializer.Deserialize<ASType>(PageJson);

        result.Should().BeOfType<ASOrderedCollectionPage<ASType>>();
        result?.Id.Should().Be("https://enby.life/users/9fpnzspgtp/outbox?page=true");
        result.As<ASOrderedCollectionPage<ASType>>().IsPaged.Should().BeFalse();
        result.As<ASOrderedCollectionPage<ASType>>().HasItems.Should().BeTrue();
        result.As<ASOrderedCollectionPage<ASType>>().TotalItems.Should().Be(5);
        result.As<ASOrderedCollectionPage<ASType>>().Items.Should().HaveCount(5);
    }

    private readonly IJsonLdSerializer _jsonLdSerializer;

    public CalckeyOutboxPageTests()
    {
        var jsonTypeInfoCache = new JsonTypeInfoCache();
        var asTypeInfoCache = new ASTypeInfoCache(jsonTypeInfoCache);
        asTypeInfoCache.RegisterAllAssemblies();

        _jsonLdSerializer = new JsonLdSerializer(asTypeInfoCache, jsonTypeInfoCache);
    }
}