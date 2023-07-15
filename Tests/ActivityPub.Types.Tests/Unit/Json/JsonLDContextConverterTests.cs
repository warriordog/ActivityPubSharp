using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Json;

public abstract class JsonLDContextConverterTests : JsonConverterTests<JsonLDContext, JsonLDContextConverter>
{
    protected override JsonLDContextConverter ConverterUnderTest { get; set; } = new();

    public class ReadShould : JsonLDContextConverterTests
    {
        [Fact]
        public void PassThroughNull()
        {
            var json = "null"u8;
            var result = Read(json);
            result.Should().BeNull();
        }

        [Fact]
        public void ParseStringAsLink()
        {
            var json = "\"https://example.com/context.jsonld\""u8;
            
            var result = Read(json);
            
            result.Should().NotBeNull();
            result?.IsExternal.Should().BeTrue();
            result?.ExternalLink?.Should().Be("https://example.com/context.jsonld");
        }


        [Fact]
        public void ParseObjectAsTerms()
        {
            var json = "{\"name\":\"https://example.com/name\"}"u8;
            
            var result = Read(json);
            
            result.Should().NotBeNull();
            result?.IsEmbedded.Should().BeTrue();
            result?.Terms.Should().NotBeNull();
            result?.Terms?.Should().HaveCount(1);
            result?.Terms?.Should().ContainKey("name");
            result?.Terms?["name"].Id.Should().Be("https://example.com/name");
        }

        [Fact]
        public void ThrowJsonException_WhenInputIsUnsupported()
        {
            Assert.Throws<JsonException>(() =>
            {
                var json = "[]"u8;
                Read(json);
            });
        }
    }

    public class WriteShould : JsonLDContextConverterTests
    {
        [Fact]
        public void WriteLinkAsString()
        {
            var input = new JsonLDContext("https://example.com/context.jsonld");
            
            var json = Write(input);
            
            json.Should().Be("\"https://example.com/context.jsonld\"");
        }

        [Fact]
        public void WriteTermsAsObject()
        {
            var input = new JsonLDContext(new Dictionary<string, JsonLDTerm>
            {
                {
                    "name",
                    new JsonLDTerm
                    {
                        Id = "https://example.com/name"
                    }
                }
            });

            var json = Write(input);

            json.Should().Be("{\"name\":\"https://example.com/name\"}");
        }
    }
}