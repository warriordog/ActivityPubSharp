using ActivityPub.Types.Extended.Object;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public class SimpleObjectSerializationTests
{
    public class EmptyObject
    {
        protected readonly ASObject ObjectUnderTest = new();
        protected Dictionary<string, JsonElement> JsonUnderTest => SerializeObject(ObjectUnderTest);

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

    public class FullObject
    {
        [Fact]
        public void ShouldIncludeAllProperties()
        {
            var obj = new ASObject()
            {
                Attachment = new LinkableList<ASObject> { new ASObject() },
                Audience = new LinkableList<ASObject> { new ASObject() },
                BCC = new LinkableList<ASObject> { new ASObject() },
                BTo = new LinkableList<ASObject> { new ASObject() },
                CC = new LinkableList<ASObject> { new ASObject() },
                Context = "context", // this is the worst field name
                Generator = new ASObject(),
                Icon = new ImageObject(),
                Image = new ImageObject(),
                InReplyTo = new ASObject(),
                Location = new ASObject(),
                Replies = new ASCollection(),
                Tag = new LinkableList<ASObject> { new ASObject() },
                To = new LinkableList<ASObject> { new ASObject() },
                Url = new LinkableList<ASLink> { new ASLink { HRef = "https://example.com" } },
                Content = new NaturalLanguageString("content"),
                Duration = "PT5S",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Published = DateTime.Now,
                Summary = new NaturalLanguageString("summary"),
                Updated = DateTime.Now,
                Source = new ASObject(),
                Likes = new ASCollection(),
                Shares = new ASCollection()
            };

            var json = JsonSerializer.Serialize(obj, JsonLdSerializerOptions.Default);

            json.Should().Contain("\"attachment\":");
            json.Should().Contain("\"audience\":");
            json.Should().Contain("\"bcc\":");
            json.Should().Contain("\"bto\":");
            json.Should().Contain("\"cc\":");
            json.Should().Contain("\"context\":"); // I hate this property NAME IT SOMETHING ELSE >:(
            json.Should().Contain("\"generator\":");
            json.Should().Contain("\"icon\":");
            json.Should().Contain("\"image\":");
            json.Should().Contain("\"inReplyTo\":");
            json.Should().Contain("\"location\":");
            json.Should().Contain("\"replies\":");
            json.Should().Contain("\"tag\":");
            json.Should().Contain("\"to\":");
            json.Should().Contain("\"url\":");
            json.Should().Contain("\"content\":");
            json.Should().Contain("\"duration\":");
            json.Should().Contain("\"startTime\":");
            json.Should().Contain("\"endTime\":");
            json.Should().Contain("\"published\":");
            json.Should().Contain("\"summary\":");
            json.Should().Contain("\"updated\":");
            json.Should().Contain("\"source\":");
            json.Should().Contain("\"likes\":");
            json.Should().Contain("\"shares\":");
        }
    }

    public class Subclass
    {
        [Fact]
        public void ShouldSerializeToCorrectType()
        {
            var obj = new ImageObject();
            var json = SerializeObject(obj);
            json["type"].GetString().Should().Be(ImageObject.ImageType);
        }
    }

    private static Dictionary<string, JsonElement> SerializeObject(object obj)
    {
        var json = JsonSerializer.Serialize(obj, JsonLdSerializerOptions.Default);
        return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json, JsonLdSerializerOptions.Default) ?? throw new ApplicationException("Deserialization failed!");
    }
}