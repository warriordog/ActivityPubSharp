// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Conversion.Converters;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Json;

public abstract class JsonLDTermConverterTests : JsonConverterTests<JsonLDTerm, JsonLDTermConverter>
{
    protected override JsonLDTermConverter ConverterUnderTest { get; set; } = new();

    public class ReadShould : JsonLDTermConverterTests
    {
        [Fact]
        public void PassThroughNull()
        {
            var json = "null"u8;
            var result = Read(json);
            result.Should().BeNull();
        }

        [Fact]
        public void WrapString()
        {
            var json = "\"https://example.com/context.jsonld\""u8;

            var result = Read(json);

            result.Should().NotBeNull();
            result?.Id.Should().Be("https://example.com/context.jsonld");
        }

        [Fact]
        public void DeserializeObject()
        {
            var json = "{\"@id\":\"https://example.com/context.jsonld\",\"@type\":\"@id\"}"u8;

            var result = Read(json);

            result.Should().NotBeNull();
            result.Should().BeOfType<JsonLDExpandedTerm>();
            result.As<JsonLDExpandedTerm>().Id.Should().Be("https://example.com/context.jsonld");
            result.As<JsonLDExpandedTerm>().Type.Should().Be("@id");
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

    public class WriteShould : JsonLDTermConverterTests
    {
        [Fact]
        public void WriteId()
        {
            var input = new JsonLDTerm()
            {
                Id = "https://example.com/context.jsonld"
            };

            var json = Write(input);

            json.Should().Be("\"https://example.com/context.jsonld\"");
        }
    }
}