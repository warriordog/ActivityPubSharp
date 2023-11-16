// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Conversion.Converters;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Conversion.Converters;

internal abstract class ASUriConverterTests : JsonConverterTests<ASUri, ASUriConverter>
{
    protected override ASUriConverter ConverterUnderTest { get; set; } = new();

    public class ReadShould : ASUriConverterTests
    {
        [Fact]
        public void ThrowJsonException_WhenInputIsNotString()
        {
            Assert.Throws<JsonException>(
                () =>
                {
                    var json = "{}"u8;
                    Read(json);
                }
            );
        }

        [Fact]
        public void WrapInputString()
        {
            var json = "\"https://example.com/some.uri\""u8;
            var asUri = Read(json);
            asUri?.ToString().Should().Be("https://example.com/some.uri");
        }
    }

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