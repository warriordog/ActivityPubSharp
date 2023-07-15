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
         "id": "https://enby.life/users/9fpwmts9tv",
         "inbox": "https://enby.life/users/9fpwmts9tv/inbox",
         "outbox": "https://enby.life/users/9fpwmts9tv/outbox",
         "followers": "https://enby.life/users/9fpwmts9tv/followers",
         "following": "https://enby.life/users/9fpwmts9tv/following",
         "featured": "https://enby.life/users/9fpwmts9tv/collections/featured",
         "sharedInbox": "https://enby.life/inbox",
         "endpoints": {
          "sharedInbox": "https://enby.life/inbox"
         },
         "url": "https://enby.life/@hazelnoot",
         "preferredUsername": "hazelnoot",
         "name": "Hazelnoot ALT",
         "summary": "<p><span>temporary(?) alt of </span><a href=\"https://enby.life/@hazel@koehlercode.dev\" class=\"u-url mention\">@hazel@koehlercode.dev</a></p>",
         "icon": {
          "type": "Image",
          "url": "https://enby.life/files/a15e1514-0c3a-48a1-8480-bb5b8e880cef",
          "sensitive": false,
          "name": null
         },
         "image": null,
         "tag": [],
         "manuallyApprovesFollowers": true,
         "discoverable": true,
         "publicKey": {
          "id": "https://enby.life/users/9fpwmts9tv#main-key",
          "type": "Key",
          "owner": "https://enby.life/users/9fpwmts9tv",
          "publicKeyPem": "-----BEGIN PUBLIC KEY-----\nMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAt9jp/+UpkimvvfK/RTSJ\n5CfwLwdWWh36YcAsxzRyOSs84q59QodWIlQOM4LSBOK+oy+2bZsLFrYDhA/QxLk8\nLnOV8/hlH0pYxQ7Pj3UV4v/hjslYaQJE/peAWrRfIQPO9uUkCkU3EKBVj5X6l6sT\nUhCn2T7kswU6Bf7HH54WFqUPpHhLGuUEdsOjMp9RP3AUbuG+s9SkzdwzhElTyehc\niMj12go/nPoWt09NBHZiALMlSL9AE/DgxfCd2woG0AwM4iz7oUzgdw4qNR+0LQEf\nauBTNQWlagbPYfkG35tzLrEodkhpIzt8OINdb7fnuoZS7Vm3t000cyyq4h7tijWC\nF7VM3Th2R+jN0GCng31Nuj0aHDsdYSFRgcPSHjcVLEQvrP0TVgobpNBwnh6iNjOs\nkYAm/59U/AQbbq3VB74GuvxRfKzf+nTnZn8Bqmj5LX0YzAaZFvvzs1kKWJfmmvca\nRVAnGK2bsL/ogecwSD5Pa0TCPLMI79yzS33YX+BcOedVBU2GtsVINYSaYHVdhrod\naeE0Lcgm9dlzYtgzUYlMfPQ5WnUMxJrEDfbdEnJGIQPmSsh0tIbVWQJfBmRwdw2h\nSyquOJbj+dgIysqHVgee0T1IjjbK9Fmq9Ip3bgRxjrx/JcFUEzXyzbuX9ViPSIWX\n4OHqODa/m/v5ch2pJyorpxsCAwEAAQ==\n-----END PUBLIC KEY-----\n"
         },
         "isCat": false,
         "attachment": [
          {
           "type": "PropertyValue",
           "name": "Me",
           "value": "<a href=\"https://mastodon.koehlercode.dev/@hazel\" rel=\"me nofollow noopener\" target=\"_blank\">https://mastodon.koehlercode.dev/@hazel</a>"
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
        result?.Id.Should().Be("https://enby.life/users/9fpwmts9tv");
        result.As<PersonActor>().Inbox.HRef.Should().Be("https://enby.life/users/9fpwmts9tv/inbox");
    }

    private readonly IJsonLdSerializer _jsonLdSerializer;

    public CalckeyActorTests()
    {
        // TODO remove this once we support DI in tests
        var jsonTypeInfoCache = new JsonTypeInfoCache();
        var asTypeInfoCache = new ASTypeInfoCache(jsonTypeInfoCache);
        asTypeInfoCache.RegisterAllAssemblies();

        _jsonLdSerializer = new JsonLdSerializer(asTypeInfoCache, jsonTypeInfoCache);
    }
}