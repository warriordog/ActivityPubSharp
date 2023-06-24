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
            _objectUnderTest = new Lazy<ASType>(() => JsonSerializer.Deserialize<ASObject>(JsonUnderTest, JsonLdSerializerOptions.Default) ?? throw new ApplicationException("Deserialization failed!"));
        }
    }

    private string _jsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object"}""";

    // Cached for performance
    private ASType ObjectUnderTest => _objectUnderTest.Value;
    private Lazy<ASType> _objectUnderTest;

    protected SimpleObjectDeserializationTests()
    {
        _objectUnderTest = new Lazy<ASType>(() => JsonSerializer.Deserialize<ASObject>(JsonUnderTest, JsonLdSerializerOptions.Default) ?? throw new ApplicationException("Deserialization failed!"));
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
}