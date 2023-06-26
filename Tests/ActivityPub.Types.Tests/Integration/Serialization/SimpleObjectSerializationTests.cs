using ActivityPub.Types.Collection;
using ActivityPub.Types.Extended.Actor;
using ActivityPub.Types.Extended.Object;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public abstract class SimpleObjectSerializationTests
{
    private ASObject ObjectUnderTest
    {
        get => _objectUnderTest;
        set
        {
            _objectUnderTest = value;

            // TODO maybe we could create a ResettableLazy, then this wouldn't need to be duplicated
            _jsonUnderTest = new Lazy<Dictionary<string, JsonElement>>(() => SerializeObject(ObjectUnderTest));
        }
    }

    private ASObject _objectUnderTest = new();

    // This is cached for performance
    private Dictionary<string, JsonElement> JsonUnderTest => _jsonUnderTest.Value;
    private Lazy<Dictionary<string, JsonElement>> _jsonUnderTest;

    protected SimpleObjectSerializationTests()
    {
        _jsonUnderTest = new Lazy<Dictionary<string, JsonElement>>(() => SerializeObject(ObjectUnderTest));
    }

    public class EmptyObject : SimpleObjectSerializationTests
    {
        [Fact]
        public void ShouldIncludeContext()
        {
            JsonUnderTest.Should().ContainKey("@context");
            JsonUnderTest["@context"].ValueKind.Should().Be(JsonValueKind.String);
            JsonUnderTest["@context"].GetString().Should().Be("https://www.w3.org/ns/activitystreams");
        }

        [Fact]
        public void ShouldIncludeType()
        {
            JsonUnderTest.Should().ContainKey("type");
            JsonUnderTest["type"].ValueKind.Should().Be(JsonValueKind.String);
            JsonUnderTest["type"].GetString().Should().Be("Object");
        }
    }

    public class Subclass : SimpleObjectSerializationTests
    {
        [Fact]
        public void ShouldSerializeToCorrectType()
        {
            ObjectUnderTest = new ImageObject();
            JsonUnderTest["type"].GetString().Should().Be(ImageObject.ImageType);
        }

        [Fact]
        public void ShouldIncludePropertiesFromBaseTypes()
        {
            ObjectUnderTest = new PersonActor()
            {
                // From ASActor
                Inbox = new ASLink { HRef = "https://example.com/actor/inbox" },
                Outbox = new ASLink { HRef = "https://example.com/actor/outbox" },

                // From ASObject
                Image = new ImageObject(),

                // From ASType
                Id = "https://example.com/actor/id"
            };

            JsonUnderTest["inbox"].Deserialize<ASLink>()?.HRef.Should().Be("https://example.com/actor/inbox");
            JsonUnderTest["outbox"].Deserialize<ASLink>()?.HRef.Should().Be("https://example.com/actor/outbox");
            JsonUnderTest["image"].Deserialize<ImageObject>().Should().NotBeNull();
            JsonUnderTest["id"].GetString().Should().Be("https://example.com/actor/id");
        }
    }

    public class FullObject : SimpleObjectSerializationTests
    {
        [Fact]
        public void ShouldIncludeAllProperties()
        {
            ObjectUnderTest = new ASObject()
            {
                // From ASObject
                Attachment = new LinkableList<ASObject> { new ASObject() },
                Audience = new LinkableList<ASObject> { new ASObject() },
                BCC = new LinkableList<ASObject> { new ASObject() },
                BTo = new LinkableList<ASObject> { new ASObject() },
                CC = new LinkableList<ASObject> { new ASObject() },
                Context = (ASLink)"https://example.com/some/context", // this is the worst field name
                Generator = new ASObject(),
                Icon = new ImageObject(),
                Image = new ImageObject(),
                InReplyTo = new ASObject(),
                Location = new ASObject(),
                Replies = new ASUnpagedCollection<ASObject>
                {
                    TotalItems = 1,
                    Items = new LinkableList<ASObject>
                    {
                        new ASObject()
                    }
                },
                Tag = new LinkableList<ASObject> { new ASObject() },
                To = new LinkableList<ASObject> { new ASObject() },
                Url = "https://example.com",
                Content = new NaturalLanguageString("content"),
                Duration = "PT5S",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Published = DateTime.Now,
                Summary = new NaturalLanguageString("summary"),
                Updated = DateTime.Now,
                Source = new ASObject(),
                Likes = new ASUnpagedCollection<ASObject>
                {
                    TotalItems = 1,
                    Items = new LinkableList<ASObject>
                    {
                        new ASObject()
                    }
                },
                Shares = new ASUnpagedCollection<ASObject>
                {
                    TotalItems = 1,
                    Items = new LinkableList<ASObject>
                    {
                        new ASObject()
                    }
                },

                // From ASType
                Id = "https://example.com/some.uri",
                AttributedTo = new LinkableList<ASObject> { new ASObject() },
                Preview = new ASObject(),
                Name = new NaturalLanguageString("name"),
                MediaType = new ASObject()
            };
            
            // From ASObject
            JsonUnderTest.Should().ContainKey("attachment");
            JsonUnderTest.Should().ContainKey("audience");
            JsonUnderTest.Should().ContainKey("bcc");
            JsonUnderTest.Should().ContainKey("bto");
            JsonUnderTest.Should().ContainKey("cc");
            JsonUnderTest.Should().ContainKey("context"); // I hate this property NAME IT SOMETHING ELSE >:(
            JsonUnderTest.Should().ContainKey("generator");
            JsonUnderTest.Should().ContainKey("icon");
            JsonUnderTest.Should().ContainKey("image");
            JsonUnderTest.Should().ContainKey("inReplyTo");
            JsonUnderTest.Should().ContainKey("location");
            JsonUnderTest.Should().ContainKey("replies");
            JsonUnderTest.Should().ContainKey("tag");
            JsonUnderTest.Should().ContainKey("to");
            JsonUnderTest.Should().ContainKey("url");
            JsonUnderTest.Should().ContainKey("content");
            JsonUnderTest.Should().ContainKey("duration");
            JsonUnderTest.Should().ContainKey("startTime");
            JsonUnderTest.Should().ContainKey("endTime");
            JsonUnderTest.Should().ContainKey("published");
            JsonUnderTest.Should().ContainKey("summary");
            JsonUnderTest.Should().ContainKey("updated");
            JsonUnderTest.Should().ContainKey("source");
            JsonUnderTest.Should().ContainKey("likes");
            JsonUnderTest.Should().ContainKey("shares");

            // From ASType
            JsonUnderTest.Should().ContainKey("id");
            JsonUnderTest.Should().ContainKey("attributedTo");
            JsonUnderTest.Should().ContainKey("preview");
            JsonUnderTest.Should().ContainKey("name");
            JsonUnderTest.Should().ContainKey("mediaType");
        }
    }

    private static Dictionary<string, JsonElement> SerializeObject(object obj)
    {
        var json = JsonSerializer.Serialize(obj, JsonLdSerializerOptions.Default);
        return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json, JsonLdSerializerOptions.Default) ?? throw new ApplicationException("Deserialization failed!");
    }
}