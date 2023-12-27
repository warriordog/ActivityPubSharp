// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Util;

public abstract class FlagWithTimestampTests
{
    private FlagWithTimestamp? FlagUnderTest { get; set; }

    public class ValueShould : FlagWithTimestampTests
    {
        [Fact]
        public void ConvertFromTrue()
        {
            FlagUnderTest = true;
            FlagUnderTest.Value.Should().BeTrue();
        }

        [Fact]
        public void ConvertFromFalse()
        {
            FlagUnderTest = false;
            FlagUnderTest.Value.Should().BeFalse();
        }

        [Fact]
        public void ConvertToTrue()
        {
            bool value = new FlagWithTimestamp { Value = true };
            value.Should().BeTrue();
        }

        [Fact]
        public void ConvertToFalse()
        {
            bool value = new FlagWithTimestamp { Value = false };
            value.Should().BeFalse();
        }

        [Fact]
        public void SetTimeStampToNull_WhenSetToFalse()
        {
            FlagUnderTest = new DateTime(2024, 1, 1);
            FlagUnderTest.Value = false;
            FlagUnderTest.Timestamp.Should().BeNull();
        }

        [Fact]
        public void IgnoreTimestamp_WhenSetToTrue()
        {
            FlagUnderTest = new DateTime(2024, 1, 1);
            FlagUnderTest.Value = true;
            FlagUnderTest.Timestamp.Should().NotBeNull();
        }
    }

    public class TimestampShould : FlagWithTimestampTests
    {
        [Fact]
        public void ConvertFromTimestamp()
        {
            var timestamp = new DateTime(2024, 1, 1);
            FlagUnderTest = timestamp;
            FlagUnderTest.Timestamp.Should().Be(timestamp);
        }

        [Fact]
        public void ConvertToNull()
        {
            DateTime? timestamp = new FlagWithTimestamp { Timestamp = null };
            timestamp.Should().BeNull();
        }

        [Fact]
        public void ConvertToTimestamp()
        {
            var expectedTimestamp = new DateTime(2024, 1, 1);
            DateTime? actualTimestamp = new FlagWithTimestamp { Timestamp = expectedTimestamp };
            actualTimestamp.Should().Be(expectedTimestamp);
        }

        [Fact]
        public void SetValueToTrue_WhenSetToTimestamp()
        {
            FlagUnderTest = false;
            FlagUnderTest.Timestamp = new DateTime(2024, 1, 1);
            FlagUnderTest.Value.Should().BeTrue();
        }

        [Fact]
        public void SetValueToFalse_WhenSetToNull()
        {
            FlagUnderTest = true;
            FlagUnderTest.Timestamp = null;
            FlagUnderTest.Value.Should().BeFalse();
        }
    }
}