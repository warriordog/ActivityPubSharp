// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Tests.Unit;

public abstract class ASTypeTests
{
    private ASType ObjectUnderTest { get; set; } = new StubASType();

    public class JsonLdContextsShould : ASTypeTests
    {
        [Fact]
        public void ContainASContext_ByDefault()
        {
            ObjectUnderTest.JsonLdContexts.ContextObjects.Should().Contain("https://www.w3.org/ns/activitystreams");
        }
    }

    public class TypesShould : ASTypeTests
    {
        [Fact]
        public void ContainTypeName_ByDefault()
        {
            ObjectUnderTest.Types.Should().Contain(StubASType.StubType);
        }
    }

    public class IsAnonymousShould : ASTypeTests
    {
        [Fact]
        public void BeTrue_ByDefault()
        {
            ObjectUnderTest.IsAnonymous.Should().BeTrue();
        }

        [Fact]
        public void BeTrue_WhenIdIsNull()
        {
            ObjectUnderTest.Id = null;
            ObjectUnderTest.IsAnonymous.Should().BeTrue();
        }

        [Fact]
        public void BeTrue_WhenIdIsEmpty()
        {
            ObjectUnderTest.Id = "";
            ObjectUnderTest.IsAnonymous.Should().BeTrue();
        }

        [Fact]
        public void BeTrue_WhenIdIsWhitespace()
        {
            ObjectUnderTest.Id = " ";
            ObjectUnderTest.IsAnonymous.Should().BeTrue();
        }

        [Fact]
        public void BeFalse_WhenIdIsSet()
        {
            ObjectUnderTest.Id = "https://example.com/some/id";
            ObjectUnderTest.IsAnonymous.Should().BeFalse();
        }
    }

    private class StubASType : ASType
    {
        public const string StubType = "Stub";
        public StubASType() : base(StubType) {}
    }
}