namespace ActivityPub.Types.Tests.Integration.Serialization;

public class SimpleObjectSerializationTests
{
    public class EmptyObject
    {
        protected readonly ASObject ObjectUnderTest = new();
        protected Dictionary<string, object?> JsonUnderTest => SerializeObject(ObjectUnderTest);
        
        [Fact]
        public void ShouldIncludeContext()
        {
            JsonUnderTest.Should().Contain("@context", "https://www.w3.org/ns/activitystreams");
        }

        [Fact]
        public void ShouldIncludeType()
        {
            JsonUnderTest.Should().Contain("type", "Object");
        }

        [Fact]
        public void ShouldNotIncludeOtherFields()
        {
            JsonUnderTest
                .Where(pair => pair.Value != null) // Default serializer leaves empty fields as "null", so we have to remove them
                .Should()
                .HaveCount(2); // This needs to be updated if we ever add more "mandatory" fields
        }
    }
    
    // TODO rest of tests here

    protected static Dictionary<string, object?> SerializeObject(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return JsonSerializer.Deserialize<Dictionary<string, object?>>(json) ?? throw new ApplicationException("Deserialization failed!");
    }
}