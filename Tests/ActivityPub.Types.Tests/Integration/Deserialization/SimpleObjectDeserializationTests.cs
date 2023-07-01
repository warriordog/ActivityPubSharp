using ActivityPub.Types.Extended.Actor;
using ActivityPub.Types.Extended.Object;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public abstract class SimpleObjectDeserializationTests
{
    private string JsonUnderTest
    {
        get => _jsonUnderTest;
        set
        {
            _jsonUnderTest = value;
            _objectUnderTest = new Lazy<ASObject>(() => JsonSerializer.Deserialize<ASObject>(JsonUnderTest, JsonLdSerializerOptions.Default) ?? throw new ApplicationException("Deserialization failed!"));
        }
    }

    private string _jsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object"}""";

    // Cached for performance
    private ASObject ObjectUnderTest => _objectUnderTest.Value;
    private Lazy<ASObject> _objectUnderTest;

    protected SimpleObjectDeserializationTests()
    {
        _objectUnderTest = new Lazy<ASObject>(() => JsonSerializer.Deserialize<ASObject>(JsonUnderTest, JsonLdSerializerOptions.Default) ?? throw new ApplicationException("Deserialization failed!"));
    }

    public class EmptyObject : SimpleObjectDeserializationTests
    {
        [Fact]
        public void ShouldIncludeContext()
        {
            ObjectUnderTest.JsonLdContexts.Should().Contain("https://www.w3.org/ns/activitystreams");
        }

        [Fact]
        public void ShouldIncludeType()
        {
            ObjectUnderTest.Types.Should().Contain("Object");
        }
    }

    public class Subclass : SimpleObjectDeserializationTests
    {
        [Fact]
        public void ShouldDeserializeToCorrectType()
        {
            JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Image"}""";
            ObjectUnderTest.Should().BeOfType<ImageObject>();
        }

        [Fact]
        public void ShouldIncludePropertiesFromBaseTypes()
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
                "id": "https://example.com/actor/id"
            }
            """;

            ObjectUnderTest.Should().BeOfType<PersonActor>();
            var personUnderTest = (PersonActor)ObjectUnderTest;
            personUnderTest.Inbox.HRef.Should().Be("https://example.com/actor/inbox");
            personUnderTest.Outbox.HRef.Should().Be("https://example.com/actor/outbox");
            personUnderTest.Image.Should().NotBeNull();
            personUnderTest.Id.Should().Be("https://example.com/actor/id");
        }
    }

    public class FullObject : SimpleObjectDeserializationTests
    {
        [Fact]
        public void ShouldIncludeAllProperties()
        {
            JsonUnderTest =
            """
                {
                    "attachment":[{}],
                    "audience":[{}],
                    "bcc":[{}],
                    "bto":[{}],
                    "cc":[{}],
                    "context":{},
                    "generator":{},
                    "icon":{"type":"Image"},
                    "image":{"type":"Image"},
                    "inReplyTo":{},
                    "location":{},
                    "replies":{
                        "type":"Collection",
                        "totalItems":1,
                        "items":[{}]
                    },
                    "tag":[{}],
                    "to":[{}],
                    "url":"https://example.com",
                    "content":"content",
                    "duration":"PT5S",
                    "startTime":"2023-06-26T21:30:09.2872331-04:00",
                    "endTime":"2023-06-26T21:30:09.2873668-04:00",
                    "published":"2023-06-26T21:30:09.2874915-04:00",
                    "summary":"summary",
                    "updated":"2023-06-26T21:30:09.2877318-04:00",
                    "source":{},
                    "likes":"https://example.com/likes.collection",
                    "shares":"https://example.com/shares.collection",
                    "type":"Object",
                    "@context":"https://www.w3.org/ns/activitystreams",
                    "id":"https://example.com/some.uri",
                    "attributedTo":[{}],
                    "preview":{},
                    "name":"name",
                    "mediaType":{}
                }
            """;

            ObjectUnderTest.Should().BeOfType<ASObject>();
            ObjectUnderTest.Attachment.Should().HaveCount(1);
            ObjectUnderTest.Audience.Should().HaveCount(1);
            ObjectUnderTest.BCC.Should().HaveCount(1);
            ObjectUnderTest.BTo.Should().HaveCount(1);
            ObjectUnderTest.CC.Should().HaveCount(1);
            ObjectUnderTest.Context.Should().NotBeNull();
            ObjectUnderTest.Generator.Should().NotBeNull();
            ObjectUnderTest.Icon.Should().NotBeNull();
            ObjectUnderTest.Image.Should().NotBeNull();
            ObjectUnderTest.InReplyTo.Should().NotBeNull();
            ObjectUnderTest.Location.Should().NotBeNull();
            
            ObjectUnderTest.Tag.Should().HaveCount(1);
            ObjectUnderTest.To.Should().HaveCount(1);
            ObjectUnderTest.Url.Should().NotBeNull();
            ObjectUnderTest.Content.Should().NotBeNull();
            ObjectUnderTest.Duration.Should().NotBeNull();
            ObjectUnderTest.StartTime.Should().NotBeNull();
            ObjectUnderTest.EndTime.Should().NotBeNull();
            ObjectUnderTest.Published.Should().NotBeNull();
            ObjectUnderTest.Summary.Should().NotBeNull();
            ObjectUnderTest.Updated.Should().NotBeNull();
            ObjectUnderTest.Source.Should().NotBeNull();
            ObjectUnderTest.Id.Should().NotBeNull();
            ObjectUnderTest.AttributedTo.Should().HaveCount(1);
            ObjectUnderTest.Preview.Should().NotBeNull();
            ObjectUnderTest.Name.Should().NotBeNull();
            ObjectUnderTest.MediaType.Should().NotBeNull();
            
            ObjectUnderTest.Replies.Should().NotBeNull();
            ObjectUnderTest.Replies!.HasItems.Should().BeTrue();
            ObjectUnderTest.Replies!.HasObjectItems.Should().BeTrue();
            ObjectUnderTest.Replies!.TotalItems.Should().Be(1);
            ObjectUnderTest.Replies!.Items!.Count.Should().Be(1);
            
            ObjectUnderTest.Likes?.HasLink.Should().BeTrue();
            ObjectUnderTest.Shares?.HasLink.Should().BeTrue();
        }
    }
}