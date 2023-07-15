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
          "id": "https://enby.life/users/9fpwmts9tv/outbox?page=true",
          "partOf": "https://enby.life/users/9fpwmts9tv/outbox",
          "type": "OrderedCollectionPage",
          "totalItems": 375,
          "orderedItems": [
            {
              "id": "https://enby.life/notes/9h7n0htfhd9pcmgf/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Create",
              "published": "2023-07-15T13:58:14.067Z",
              "object": {
                "id": "https://enby.life/notes/9h7n0htfhd9pcmgf",
                "type": "Note",
                "attributedTo": "https://enby.life/users/9fpwmts9tv",
                "summary": "vaguely lewd coding joke",
                "content": "<p><span>QT: </span><i><span>added CW</span></i><span><br><br>RE: </span><a href=\"https://queer.af/users/aiju/statuses/110192716256457719\">https://queer.af/users/aiju/statuses/110192716256457719</a></p>",
                "_misskey_content": "QT: *added CW*",
                "source": {
                  "content": "QT: *added CW*",
                  "mediaType": "text/x.misskeymarkdown"
                },
                "_misskey_quote": "https://queer.af/users/aiju/statuses/110192716256457719",
                "quoteUri": "https://queer.af/users/aiju/statuses/110192716256457719",
                "quoteUrl": "https://queer.af/users/aiju/statuses/110192716256457719",
                "published": "2023-07-15T13:58:14.067Z",
                "to": [
                  "https://www.w3.org/ns/activitystreams#Public"
                ],
                "cc": [
                  "https://enby.life/users/9fpwmts9tv/followers"
                ],
                "inReplyTo": null,
                "attachment": [],
                "sensitive": true,
                "tag": []
              },
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ]
            },
            {
              "id": "https://enby.life/notes/9h7mun9lc2tyangr/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-15T13:53:41.193Z",
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "object": "https://misskey.heonian.org/notes/9h7mdunpcl"
            },
            {
              "id": "https://enby.life/notes/9h7mrmyqwklp7btm/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-15T13:51:20.834Z",
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "object": "https://girldick.gay/users/anna/statuses/110718240495080651"
            },
            {
              "id": "https://enby.life/notes/9h7mqpgtnmdogi1p/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Create",
              "published": "2023-07-15T13:50:37.421Z",
              "object": {
                "id": "https://enby.life/notes/9h7mqpgtnmdogi1p",
                "type": "Note",
                "attributedTo": "https://enby.life/users/9fpwmts9tv",
                "summary": null,
                "content": "<p><a href=\"https://girldick.gay/@anna\" class=\"u-url mention\">@anna@girldick.gay</a><span> </span><a href=\"https://social.vlhl.dev/users/navi\" class=\"u-url mention\">@navi@social.vlhl.dev</a><span> </span><a href=\"https://astolfo.social/@natty\" class=\"u-url mention\">@natty@astolfo.social</a><span> PHP? </span>​:neofox_laugh:​</p>",
                "_misskey_content": "@anna@girldick.gay @navi@social.vlhl.dev @natty@astolfo.social PHP? :neofox_laugh:",
                "source": {
                  "content": "@anna@girldick.gay @navi@social.vlhl.dev @natty@astolfo.social PHP? :neofox_laugh:",
                  "mediaType": "text/x.misskeymarkdown"
                },
                "published": "2023-07-15T13:50:37.421Z",
                "to": [
                  "https://enby.life/users/9fpwmts9tv/followers"
                ],
                "cc": [
                  "https://www.w3.org/ns/activitystreams#Public",
                  "https://girldick.gay/users/anna",
                  "https://social.vlhl.dev/users/navi",
                  "https://astolfo.social/users/9awy7u3l76"
                ],
                "inReplyTo": "https://girldick.gay/users/anna/statuses/110718405906613277",
                "attachment": [],
                "sensitive": false,
                "tag": [
                  {
                    "type": "Mention",
                    "href": "https://astolfo.social/users/9awy7u3l76",
                    "name": "@natty@astolfo.social"
                  },
                  {
                    "type": "Mention",
                    "href": "https://social.vlhl.dev/users/navi",
                    "name": "@navi@social.vlhl.dev"
                  },
                  {
                    "type": "Mention",
                    "href": "https://girldick.gay/users/anna",
                    "name": "@anna@girldick.gay"
                  },
                  {
                    "id": "https://enby.life/emojis/neofox_laugh",
                    "type": "Emoji",
                    "name": ":neofox_laugh:",
                    "updated": "2023-07-08T02:23:38.934Z",
                    "icon": {
                      "type": "Image",
                      "mediaType": "image/png",
                      "url": "https://enby.life/files/cff2d6b8-4fb6-4b8c-bb83-219c9dcf767d"
                    }
                  }
                ]
              },
              "to": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "cc": [
                "https://www.w3.org/ns/activitystreams#Public",
                "https://girldick.gay/users/anna",
                "https://social.vlhl.dev/users/navi",
                "https://astolfo.social/users/9awy7u3l76"
              ]
            },
            {
              "id": "https://enby.life/notes/9h6yt7lhytyi9m6n/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Create",
              "published": "2023-07-15T02:40:43.445Z",
              "object": {
                "id": "https://enby.life/notes/9h6yt7lhytyi9m6n",
                "type": "Note",
                "attributedTo": "https://enby.life/users/9fpwmts9tv",
                "summary": null,
                "content": "<p><a href=\"https://mk.absturztau.be/@Stellar\" class=\"u-url mention\">@Stellar@mk.absturztau.be</a><span> pls no </span>​:neofox_cry_loud:​</p>",
                "_misskey_content": "@Stellar@mk.absturztau.be pls no :neofox_cry_loud:",
                "source": {
                  "content": "@Stellar@mk.absturztau.be pls no :neofox_cry_loud:",
                  "mediaType": "text/x.misskeymarkdown"
                },
                "published": "2023-07-15T02:40:43.445Z",
                "to": [
                  "https://www.w3.org/ns/activitystreams#Public"
                ],
                "cc": [
                  "https://enby.life/users/9fpwmts9tv/followers",
                  "https://mk.absturztau.be/users/8p82w2nz3u"
                ],
                "inReplyTo": "https://mk.absturztau.be/notes/9h6ym7q9g2",
                "attachment": [],
                "sensitive": false,
                "tag": [
                  {
                    "type": "Mention",
                    "href": "https://mk.absturztau.be/users/8p82w2nz3u",
                    "name": "@Stellar@mk.absturztau.be"
                  },
                  {
                    "id": "https://enby.life/emojis/neofox_cry_loud",
                    "type": "Emoji",
                    "name": ":neofox_cry_loud:",
                    "updated": "2023-07-08T02:34:13.970Z",
                    "icon": {
                      "type": "Image",
                      "mediaType": "image/png",
                      "url": "https://enby.life/files/82bbd230-f9ab-4fc0-a166-f5992dc42989"
                    }
                  }
                ]
              },
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers",
                "https://mk.absturztau.be/users/8p82w2nz3u"
              ]
            },
            {
              "id": "https://enby.life/notes/9h6poyrxalfzxalv/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-14T22:25:28.845Z",
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "object": "https://universeodon.com/users/cyborus/statuses/110714699375615966"
            },
            {
              "id": "https://enby.life/notes/9h6pmwoty4ls2dmk/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-14T22:23:52.829Z",
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "object": "https://eigenmagic.net/users/daedalus/statuses/110714712273340136"
            },
            {
              "id": "https://enby.life/notes/9h6p83h4f13cxhnu/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-14T22:12:21.784Z",
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "object": "https://benjojo.co.uk/u/benjojo/h/ZDJkQ4BW7x7J153t2Q"
            },
            {
              "id": "https://enby.life/notes/9h6fbgcjmxnna883/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-14T17:35:02.275Z",
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "object": "https://girlcock.club/users/ultimateanalyst/statuses/110706688497104713"
            },
            {
              "id": "https://enby.life/notes/9h6atzvdgaitllhn/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-14T15:29:29.305Z",
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "object": "https://yiff.life/users/nano/statuses/110712853286012853"
            },
            {
              "id": "https://enby.life/notes/9h6aopwmb12jlrz7/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-14T15:25:23.110Z",
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "object": "https://tech.lgbt/users/agatha/statuses/110712748844768255"
            },
            {
              "id": "https://enby.life/notes/9h5f12hydis1byz9/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-14T00:39:11.590Z",
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "object": "https://demon.social/users/vyr/statuses/110709520592470465"
            },
            {
              "id": "https://enby.life/notes/9h5brjdcdte8c3pj/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Create",
              "published": "2023-07-13T23:07:48.048Z",
              "object": {
                "id": "https://enby.life/notes/9h5brjdcdte8c3pj",
                "type": "Note",
                "attributedTo": "https://enby.life/users/9fpwmts9tv",
                "summary": "transphobia, parents",
                "content": "<p><a href=\"https://astolfo.social/@natty\" class=\"u-url mention\">@natty@astolfo.social</a><span> that's so fucked up </span>​:neofox_heart:​</p>",
                "_misskey_content": "@natty@astolfo.social that's so fucked up :neofox_heart:",
                "source": {
                  "content": "@natty@astolfo.social that's so fucked up :neofox_heart:",
                  "mediaType": "text/x.misskeymarkdown"
                },
                "published": "2023-07-13T23:07:48.048Z",
                "to": [
                  "https://enby.life/users/9fpwmts9tv/followers"
                ],
                "cc": [
                  "https://www.w3.org/ns/activitystreams#Public",
                  "https://astolfo.social/users/9awy7u3l76"
                ],
                "inReplyTo": "https://astolfo.social/notes/9h582oi0yz9xtt4r",
                "attachment": [],
                "sensitive": true,
                "tag": [
                  {
                    "type": "Mention",
                    "href": "https://astolfo.social/users/9awy7u3l76",
                    "name": "@natty@astolfo.social"
                  },
                  {
                    "id": "https://enby.life/emojis/neofox_heart",
                    "type": "Emoji",
                    "name": ":neofox_heart:",
                    "updated": "2023-07-08T02:25:42.193Z",
                    "icon": {
                      "type": "Image",
                      "mediaType": "image/png",
                      "url": "https://enby.life/files/4de4167b-355d-4ec1-9724-06f671b81311"
                    }
                  }
                ]
              },
              "to": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "cc": [
                "https://www.w3.org/ns/activitystreams#Public",
                "https://astolfo.social/users/9awy7u3l76"
              ]
            },
            {
              "id": "https://enby.life/notes/9h5760q1gjjp97zw/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-13T20:59:05.641Z",
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "object": "https://mastodon.koehlercode.dev/users/hazel/statuses/110708749797939520"
            },
            {
              "id": "https://enby.life/notes/9h53adygri6o95ec/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-13T19:10:30.952Z",
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "object": "https://ak.yass.ee/objects/73d1f814-0a07-4dce-9a28-7442721b06f0"
            },
            {
              "id": "https://enby.life/notes/9h509fxsfxjdsxd2/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-13T17:45:48.016Z",
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "object": "https://retr0.id/objects/2cf7f60d-8c6a-4c14-acfa-19a6f69eacdd"
            },
            {
              "id": "https://enby.life/notes/9h4yrjsok4wv0f70/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-13T17:03:53.592Z",
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "object": "https://kolektiva.social/users/MnemosyneSinger/statuses/110707389839513844"
            },
            {
              "id": "https://enby.life/notes/9h4x9bfdwqhy5vkq/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Announce",
              "published": "2023-07-13T16:21:43.321Z",
              "to": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ],
              "cc": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "object": "https://tech.lgbt/users/chronovore/statuses/110707683215990240"
            },
            {
              "id": "https://enby.life/notes/9h4x65qqf552r9kq/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Create",
              "published": "2023-07-13T16:19:15.986Z",
              "object": {
                "id": "https://enby.life/notes/9h4x65qqf552r9kq",
                "type": "Note",
                "attributedTo": "https://enby.life/users/9fpwmts9tv",
                "summary": null,
                "content": "<p><span>Well that's fun, if you quote a post and enter only a CW (no text) then </span><a href=\"https://enby.life/tags/Calckey\" rel=\"tag\">#Calckey</a><span> will silently </span><b><span>remove the CW</span></b><span> and convert the quote into a normal boost. </span>​:neofox_scream_stare:​</p>",
                "_misskey_content": "Well that's fun, if you quote a post and enter only a CW (no text) then #Calckey will silently **remove the CW** and convert the quote into a normal boost. :neofox_scream_stare:",
                "source": {
                  "content": "Well that's fun, if you quote a post and enter only a CW (no text) then #Calckey will silently **remove the CW** and convert the quote into a normal boost. :neofox_scream_stare:",
                  "mediaType": "text/x.misskeymarkdown"
                },
                "published": "2023-07-13T16:19:15.986Z",
                "to": [
                  "https://www.w3.org/ns/activitystreams#Public"
                ],
                "cc": [
                  "https://enby.life/users/9fpwmts9tv/followers"
                ],
                "inReplyTo": null,
                "attachment": [],
                "sensitive": false,
                "tag": [
                  {
                    "type": "Hashtag",
                    "href": "https://enby.life/tags/Calckey",
                    "name": "#Calckey"
                  },
                  {
                    "id": "https://enby.life/emojis/neofox_scream_stare",
                    "type": "Emoji",
                    "name": ":neofox_scream_stare:",
                    "updated": "2023-07-08T02:10:20.523Z",
                    "icon": {
                      "type": "Image",
                      "mediaType": "image/png",
                      "url": "https://enby.life/files/fff981ea-2368-4474-b3af-ed77b6526054"
                    }
                  }
                ]
              },
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ]
            },
            {
              "id": "https://enby.life/notes/9h4x3p4bvyumum2a/activity",
              "actor": "https://enby.life/users/9fpwmts9tv",
              "type": "Create",
              "published": "2023-07-13T16:17:21.131Z",
              "object": {
                "id": "https://enby.life/notes/9h4x3p4bvyumum2a",
                "type": "Note",
                "attributedTo": "https://enby.life/users/9fpwmts9tv",
                "summary": "slightly lewd/nsfw",
                "content": "<p><span>QT: </span><i><span>added CW</span></i><span><br><br>RE: </span><a href=\"https://meow.social/users/rinidisc/statuses/110707022691756345\">https://meow.social/users/rinidisc/statuses/110707022691756345</a></p>",
                "_misskey_content": "QT: *added CW*",
                "source": {
                  "content": "QT: *added CW*",
                  "mediaType": "text/x.misskeymarkdown"
                },
                "_misskey_quote": "https://meow.social/users/rinidisc/statuses/110707022691756345",
                "quoteUri": "https://meow.social/users/rinidisc/statuses/110707022691756345",
                "quoteUrl": "https://meow.social/users/rinidisc/statuses/110707022691756345",
                "published": "2023-07-13T16:17:21.131Z",
                "to": [
                  "https://www.w3.org/ns/activitystreams#Public"
                ],
                "cc": [
                  "https://enby.life/users/9fpwmts9tv/followers"
                ],
                "inReplyTo": null,
                "attachment": [],
                "sensitive": true,
                "tag": []
              },
              "to": [
                "https://www.w3.org/ns/activitystreams#Public"
              ],
              "cc": [
                "https://enby.life/users/9fpwmts9tv/followers"
              ]
            }
          ],
          "prev": "https://enby.life/users/9fpwmts9tv/outbox?page=true&since_id=9h7n0htfhd9pcmgf",
          "next": "https://enby.life/users/9fpwmts9tv/outbox?page=true&until_id=9h4x3p4bvyumum2a"
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
        result?.Id.Should().Be("https://enby.life/users/9fpwmts9tv/outbox?page=true");
        result.As<ASOrderedCollectionPage<ASType>>().IsPaged.Should().BeFalse();
        result.As<ASOrderedCollectionPage<ASType>>().HasItems.Should().BeTrue();
        result.As<ASOrderedCollectionPage<ASType>>().TotalItems.Should().Be(375);
        result.As<ASOrderedCollectionPage<ASType>>().Items.Should().NotBeEmpty();
    }

    private readonly IJsonLdSerializer _jsonLdSerializer;

    public CalckeyOutboxPageTests()
    {
        // TODO remove this once we support DI in tests
        var jsonTypeInfoCache = new JsonTypeInfoCache();
        var asTypeInfoCache = new ASTypeInfoCache(jsonTypeInfoCache);
        asTypeInfoCache.RegisterAllAssemblies();

        _jsonLdSerializer = new JsonLdSerializer(asTypeInfoCache, jsonTypeInfoCache);
    }
}