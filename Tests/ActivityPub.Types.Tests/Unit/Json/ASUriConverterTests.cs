using System.Text;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Json;

public abstract class ASUriConverterTests
{
    private ASUriConverter ConverterUnderTest { get; set; } = new();
    
    // Useful: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/use-utf8jsonreader
    public class ReadShould : ASUriConverterTests
    {
        [Fact]
        public void ThrowJsonException_WhenInputIsNotString()
        {
            Assert.Throws<JsonException>(() =>
            {
                var json = "{}"u8;
                var reader = new Utf8JsonReader(json);
                reader.Read();
                ConverterUnderTest.Read(ref reader, typeof(ASUri), JsonSerializerOptions.Default);
            });
        }

        [Fact]
        public void WrapInputString()
        {
            var json = "\"https://example.com/some.uri\""u8;
            var reader = new Utf8JsonReader(json);
            reader.Read();
            var asUri = ConverterUnderTest.Read(ref reader, typeof(ASUri), JsonSerializerOptions.Default);
            asUri.ToString().Should().Be("https://example.com/some.uri");
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
            
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);
            var asUri = new ASUri(Input);
            ConverterUnderTest.Write(writer, asUri, JsonSerializerOptions.Default);
            writer.Flush();

            var json = Encoding.UTF8.GetString(stream.ToArray());
            json.Should().Be(ExpectedOutput);
        }
    }
}