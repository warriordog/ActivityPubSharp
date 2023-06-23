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

        [Fact]
        public void ShouldNotIncludeOtherFields()
        {
            JsonUnderTest
                .Where(pair => pair.Value.ValueKind != JsonValueKind.Null) // Default serializer leaves empty fields as "null", so we have to remove them
                .Should()
                .HaveCount(2); // This needs to be updated if we ever add more "mandatory" fields
        }
    }
    
    // TODO rest of tests here

    protected static Dictionary<string, JsonElement> SerializeObject(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json) ?? throw new ApplicationException("Deserialization failed!");
    }
}