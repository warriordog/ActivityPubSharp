// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Conversion.Converters;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Conversion.Converters;

public abstract class FlagWithTimestampConverterTests : JsonConverterTests<FlagWithTimestamp, FlagWithTimestampConverter>
{
    protected override FlagWithTimestampConverter ConverterUnderTest { get; set; } = new();

    public class ReadShould : FlagWithTimestampConverterTests
    {
        [Fact]
        public void ConvertFromNull()
        {
            var flag = Read("null"u8);
            flag.Should().BeNull();
        }

        [Fact]
        public void ConvertFromTrue()
        {
            var flag = Read("true"u8);
            flag.Should().NotBeNull();
            flag!.Value.Should().BeTrue();
            flag.Timestamp.Should().BeNull();
        }

        [Fact]
        public void ConvertFromFalse()
        {
            var flag = Read("false"u8);
            flag.Should().NotBeNull();
            flag!.Value.Should().BeFalse();
            flag.Timestamp.Should().BeNull();
        }

        [Fact]
        public void ConvertFromDateTime()
        {
            var flag = Read("\"2024-01-01T00:00:00\""u8);
            flag.Should().NotBeNull();
            flag!.Value.Should().BeTrue();
            flag.Timestamp.Should().Be(new DateTime(2024, 1, 1));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("{}")]
        [InlineData("[]")]
        public void ThrowOnOtherInput(string json)
        {
            Assert.Throws<JsonException>(() =>
            {
                Read(json);
            });
        }
    }

    public class WriteShould : FlagWithTimestampConverterTests
    {
        [Theory]
        [InlineData(true, "true")]
        [InlineData(false, "false")]
        public void WriteValueToBool(bool value, string expectedJson)
        {
            var flag = new FlagWithTimestamp { Value = value };
            var actualJson = Write(flag);
            actualJson.Should().Be(expectedJson);
        }

        [Fact]
        public void WriteTimestampToString()
        {
            var flag = new FlagWithTimestamp { Timestamp = new DateTime(2024, 1, 1) };
            var json = Write(flag);
            json.Should().Be("\"2024-01-01T00:00:00\"");
        }

        [Fact]
        public void WriteTimestamp_WhenBothAreSet()
        {
            var flag = new FlagWithTimestamp
            {
                Timestamp = new DateTime(2024, 1, 1),
                Value = true
            };
            var json = Write(flag);
            json.Should().Be("\"2024-01-01T00:00:00\"");
        }
    }
}