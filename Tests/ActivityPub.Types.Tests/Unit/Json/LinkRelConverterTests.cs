// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Json;

public class LinkRelConverterTests : JsonConverterTests<LinkRel, LinkRelConverter>
{
    protected override LinkRelConverter ConverterUnderTest { get; set; } = new();

    public class ReadShould : LinkRelConverterTests
    {
        [Fact]
        public void PassNull()
        {
            var input = "null"u8;
            var result = Read(input);
            result.Should().BeNull();
        }

        [Fact]
        public void WrapString()
        {
            var input = "\"valid-link-rel\""u8;
            var result = Read(input);
            result?.Value.Should().Be("valid-link-rel");
        }

        [Fact]
        public void ThrowOnWrongType()
        {
            
            Assert.Throws<JsonException>(() =>
            {
                Read("[]"u8);
            });
        }

        [Fact]
        public void ThrowOnInvalidString()
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                Read("\"has space oops\""u8);
            });
        }
    }

    public class WriteShould : LinkRelConverterTests
    {
        [Fact]
        public void WriteValueAsString()
        {
            var input = new LinkRel("valid-link-rel");
            var result = Write(input);
            result.Should().Be("\"valid-link-rel\"");
        }
    }
}