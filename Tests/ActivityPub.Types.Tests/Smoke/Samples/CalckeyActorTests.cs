// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Extended.Actor;
using ActivityPub.Types.Internal.TypeInfo;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Tests.Smoke.Samples;

public class CalckeyActorTests
{
    private const string ActorJson =
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
            "type": "Person",
            "id": "https://enby.life/users/9fpnzspgtp",
            "inbox": "https://enby.life/users/9fpnzspgtp/inbox",
            "outbox": "https://enby.life/users/9fpnzspgtp/outbox",
            "followers": "https://enby.life/users/9fpnzspgtp/followers",
            "following": "https://enby.life/users/9fpnzspgtp/following",
            "featured": "https://enby.life/users/9fpnzspgtp/collections/featured",
            "sharedInbox": "https://enby.life/inbox",
            "endpoints": {
                "sharedInbox": "https://enby.life/inbox"
            },
            "url": "https://enby.life/@admin",
            "preferredUsername": "admin",
            "name": "Enby Admin",
            "summary": "<p><span>Instance admin - contact for info and technical support</span></p>",
            "icon": {
                "type": "Image",
                "url": "https://enby.life/files/a15e1514-0c3a-48a1-8480-bb5b8e880cef",
                "sensitive": false,
                "name": null
            },
            "image": null,
             "tag": [],
             "manuallyApprovesFollowers": false,
             "discoverable": true,
             "publicKey": {
                 "id": "https://enby.life/users/9fpnzspgtp#main-key",
                 "type": "Key",
                 "owner": "https://enby.life/users/9fpnzspgtp",
                 "publicKeyPem": "-----BEGIN PUBLIC KEY-----\nMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAux49fKZK3XxSActEu+Rm\nitMezjOHKhRnQXzbh/UX++nrIrxhk4DJiQSmeOSuoeO3YiwlA8fcgK6ZU8l6RYFp\nhjjVauueWMaB7mrOHCBK2QtY9kPvtyMVtM1lt1cnqbsBsZP7YNWYS+9oZKw0NOCU\nPuapmYhkS+eA1hpBEDDZ0WG74HzNUSlweFD2ssQGeNyy0lTI20LPCvHqfDHO3rI1\nZkouHcf3es2O8CFqz7RIociMFfMWQEzBkmVo8fKxwUdOvrm4u/eKc6dZ5OLKuh3+\nRo8S10zbZvOE8btKosL9HhHF1X51Qg4gbqEU06hnTCKLyyxOvpkrrTFOXCQtaDJu\nypN1xhEEZIYL7j4QfDa+LZSeslzwRT2WywCnLyX2bgIzDvZDCVKeqHxfl/Z5YrX8\nFn+75j8tX/BDyjZs8XGFeesQx5hb/WWGI2FpqHz11GVUm/rcrE9gi4Thghkn43T1\nsY23URvBcKfY5Z+YG4eCkjsoMoJnybLKhLQ08YKY3rap4XGLsvn8SG+XByw7Uk1I\nZlvl/dO+AMJKfjPcme0cj56m85eWrHfmwGXQPOicMcKz6pxQ7u0icDD5Ychm9a8t\ndkgmRv8coB8qCEU5VX5JOK/N826MzmoHX5tG9WBAdXWpDg9wKdNn75ZbbajMm+eV\n5GSCDAq4TtaPFzGu8fKeWuECAwEAAQ==\n-----END PUBLIC KEY-----" 
             },
             "isCat": false,
             "attachment": [
                 {
                     "type": "PropertyValue",
                     "name": "Test",
                     "value": "Value" 
                 }
             ],
             "vcard:Address": "United States of America"
        }
        """;

    [Fact]
    public void ShouldDeserializeWithoutCrash()
    {
        var result = _jsonLdSerializer.Deserialize<ASType>(ActorJson);
        result.Should().NotBeNull();
    }

    [Fact]
    public void ShouldSetKeyFields()
    {
        var result = _jsonLdSerializer.Deserialize<ASType>(ActorJson);

        result.Should().BeOfType<PersonActor>();
        result?.Id.Should().Be("https://enby.life/users/9fpnzspgtp");
        result.As<PersonActor>().Inbox.HRef.Should().Be("https://enby.life/users/9fpnzspgtp/inbox");
    }

    private readonly IJsonLdSerializer _jsonLdSerializer;

    public CalckeyActorTests()
    {
        var jsonTypeInfoCache = new JsonTypeInfoCache();
        var asTypeInfoCache = new ASTypeInfoCache(jsonTypeInfoCache);
        asTypeInfoCache.RegisterAllAssemblies();

        _jsonLdSerializer = new JsonLdSerializer(asTypeInfoCache, jsonTypeInfoCache);
    }
}