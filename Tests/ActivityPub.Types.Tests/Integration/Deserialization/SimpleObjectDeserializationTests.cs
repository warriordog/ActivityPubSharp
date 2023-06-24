using ActivityPub.Types.Extended.Object;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public class SimpleObjectDeserializationTests
{
    public class EmptyObject
    {
        protected const string JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object"}""";
        protected ASObject ObjectUnderTest => JsonSerializer.Deserialize<ASObject>(JsonUnderTest, JsonLdSerializerOptions.Default) ?? throw new ApplicationException("Deserialization failed!");

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

    public class Subclass
    {
        [Fact]
        public void ShouldDeserializeToCorrectType()
        {
            const string json = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Image"}""";
            var obj = JsonSerializer.Deserialize<ASType>(json, JsonLdSerializerOptions.Default)!;
            obj.Should().NotBeNull();
            obj.Should().BeOfType<ImageObject>();
        }
    }
}