using ActivityPub.Types.Collection;
using ActivityPub.Types.Internal.TypeInfo;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Tests.Smoke.Samples;

public class CalckeyOutboxTests
{
    private const string OutboxJson =
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
          "id": "https://enby.life/users/9fpwmts9tv/outbox",
          "type": "OrderedCollection",
          "totalItems": 371,
          "first": "https://enby.life/users/9fpwmts9tv/outbox?page=true",
          "last": "https://enby.life/users/9fpwmts9tv/outbox?page=true&since_id=000000000000000000000000"
        }
        """;
    [Fact]
    public void ShouldDeserializeWithoutCrash()
    {
        var result = _jsonLdSerializer.Deserialize<ASType>(OutboxJson);
        result.Should().NotBeNull();
    }

    [Fact]
    public void ShouldSetKeyFields()
    {
        var result = _jsonLdSerializer.Deserialize<ASType>(OutboxJson);

        result.Should().BeOfType<ASOrderedCollection<ASType>>();
        result?.Id.Should().Be("https://enby.life/users/9fpwmts9tv/outbox");
        result.As<ASOrderedCollection<ASType>>().IsPaged.Should().BeTrue();
        result.As<ASOrderedCollection<ASType>>().HasItems.Should().BeFalse();
        result.As<ASOrderedCollection<ASType>>().TotalItems.Should().Be(371);
    }

    private readonly IJsonLdSerializer _jsonLdSerializer;

    public CalckeyOutboxTests()
    {
        // TODO remove this once we support DI in tests
        var jsonTypeInfoCache = new JsonTypeInfoCache();
        var asTypeInfoCache = new ASTypeInfoCache(jsonTypeInfoCache);
        asTypeInfoCache.RegisterAllAssemblies();

        _jsonLdSerializer = new JsonLdSerializer(asTypeInfoCache, jsonTypeInfoCache);
    }
}