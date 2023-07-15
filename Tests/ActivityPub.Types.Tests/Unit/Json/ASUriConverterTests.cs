using System.Text;
using System.Text.Json.Serialization;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Json;

public abstract class ASUriConverterTests : JsonConverterTests<ASUri, ASUriConverter>
{
    protected override ASUriConverter ConverterUnderTest { get; set; } = new();
    
    // Useful: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/use-utf8jsonreader
    public class ReadShould : ASUriConverterTests
    {
        [Fact]
        public void ThrowJsonException_WhenInputIsNotString()
        {
            Assert.Throws<JsonException>(() =>
            {
                var json = "{}"u8;
                Read(json);
            });
        }

        [Fact]
        public void WrapInputString()
        {
            var json = "\"https://example.com/some.uri\""u8;
            var asUri = Read(json);
            asUri?.ToString().Should().Be("https://example.com/some.uri");
        }
    }

    // Useful: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/use-utf8jsonwriter
    public class WriteShould : ASUriConverterTests
    {
        [Fact]
        public void WriteUriAsString()
        {
            const string Input = "https://example.com/some.uri";
            const string ExpectedOutput = $"\"{Input}\"";
            var asUri = new ASUri(Input);
            
            var json = Write(asUri);
            
            json.Should().Be(ExpectedOutput);
        }
    }
}