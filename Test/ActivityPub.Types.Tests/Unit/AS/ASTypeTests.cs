// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using ActivityPub.Types.AS;

namespace ActivityPub.Types.Tests.Unit.AS;

public abstract class ASTypeTests
{
    private ASType ObjectUnderTest { get; } = new StubASType();

    public class JsonLdContextsShould : ASTypeTests
    {
        [Fact]
        public void ContainASContext_ByDefault()
        {
            ObjectUnderTest.TypeMap.LDContext.Should().Contain("https://www.w3.org/ns/activitystreams");
        }
    }

    public class TypesShould : ASTypeTests
    {
        [Fact]
        public void ContainTypeName_ByDefault()
        {
            ObjectUnderTest.TypeMap.ASTypes.Should().Contain(StubASType.StubType);
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

    private class StubASType : ASType, IASModel<StubASType, StubASTypeEntity, ASType>
    {
        /// <summary>
        ///     ActivityStreams type name for "Stub" types.
        /// </summary>
        public const string StubType = "Stub";
        static string IASModel<StubASType>.ASTypeName => StubType;
        
        /// <inheritdoc />
        public StubASType() => Entity = TypeMap.Extend<StubASType, StubASTypeEntity>();

        /// <inheritdoc />
        public StubASType(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
            => Entity = TypeMap.ProjectTo<StubASType, StubASTypeEntity>(isExtending);

        /// <inheritdoc />
        public StubASType(ASType existingGraph) : this(existingGraph.TypeMap) {}

        /// <inheritdoc />
        [SetsRequiredMembers]
        public StubASType(TypeMap typeMap, StubASTypeEntity? entity) : base(typeMap, null)
            => Entity = entity ?? typeMap.AsEntity<StubASType, StubASTypeEntity>();

        static StubASType IASModel<StubASType>.FromGraph(TypeMap typeMap) => new(typeMap, null);

        private StubASTypeEntity Entity { get; }
    }

    private sealed class StubASTypeEntity : ASEntity<StubASType, StubASTypeEntity> {}
}