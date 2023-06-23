namespace ActivityPub.Types.Tests.Integration.Deserialization;

public class SimpleObjectDeserializationTests
{
    public class EmptyObject
    {
        protected readonly string JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object"}""";
        protected ASObject ObjectUnderTest => JsonSerializer.Deserialize<ASObject>(JsonUnderTest) ?? throw new ApplicationException("Deserialization failed!");
        
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
}