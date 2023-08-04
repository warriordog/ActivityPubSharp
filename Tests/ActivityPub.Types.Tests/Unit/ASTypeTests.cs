// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Tests.Unit;

public abstract class ASTypeTests
{
    private ASType ObjectUnderTest { get; set; } = new StubASType();

    public class JsonLdContextsShould : ASTypeTests
    {
        [Fact]
        public void ContainASContext_ByDefault()
        {
            ObjectUnderTest.JsonLdContext.Should().Contain("https://www.w3.org/ns/activitystreams");
        }
    }

    public class TypesShould : ASTypeTests
    {
        [Fact]
        public void ContainTypeName_ByDefault()
        {
            ObjectUnderTest.Types.Should().Contain(StubASTypeEntity.StubType);
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
        private StubASTypeEntity Entity { get; }

        public StubASType() => Entity = new StubASTypeEntity { TypeMap = TypeMap };
        public StubASType(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<StubASTypeEntity>();
    }

    [ImpliesOtherEntity(typeof(ASTypeEntity))]
    private sealed class StubASTypeEntity : ASBase<StubASType>
    {
        public const string StubType = "Stub";
        public override string ASTypeName => StubType;
    }
}