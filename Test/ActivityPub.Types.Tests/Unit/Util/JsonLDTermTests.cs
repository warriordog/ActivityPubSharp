// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Util;

public abstract class JsonLDTermTests
{
    public class EqualsShould : JsonLDTermTests
    {
        [Fact]
        public void ReturnTrue_WhenIDsMatch()
        {
            var first = new JsonLDTerm { Id = "id" };
            var second = new JsonLDTerm { Id = "id" };
            first.Equals(second).Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_WhenIDsDontMatch()
        {
            var first = new JsonLDTerm { Id = "1" };
            var second = new JsonLDTerm { Id = "2" };
            first.Equals(second).Should().BeFalse();
        }

        [Fact]
        public void ReturnFalse_WhenIDsMatchButOneHasType()
        {
            var first = new JsonLDTerm { Id = "id" };
            var second = new JsonLDExpandedTerm { Id = "id", Type = "type" };
            first.Equals(second).Should().BeFalse();
        }

        [Fact]
        public void ReturnTrue_WhenIDsAndTypesMatch()
        {
            var first = new JsonLDExpandedTerm { Id = "id", Type = "type" };
            var second = new JsonLDExpandedTerm { Id = "id", Type = "type" };
            first.Equals(second).Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_WhenIDsMatchButTypesDont()
        {
            var first = new JsonLDExpandedTerm { Id = "id", Type = "t1" };
            var second = new JsonLDExpandedTerm { Id = "id", Type = "t2" };
            first.Equals(second).Should().BeFalse();
        }
    }
}