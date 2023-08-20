// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Attributes;
using ActivityPub.Types.Internal;

namespace ActivityPub.Types.Tests.Unit.Internal;

public abstract class ASTypeInfoCacheTests
{
    private ASTypeInfoCache CacheUnderTest { get; } = new();

    public class RegisterAllAssembliesShould : ASTypeInfoCacheTests
    {
        public RegisterAllAssembliesShould() => CacheUnderTest.RegisterAllAssemblies();

        [Fact]
        public void IncludeTypesPackage()
        {
            CacheUnderTest.IsKnownASType(ASObjectEntity.ObjectType).Should().BeTrue();
        }

        [Fact]
        public void IncludeOtherPackages()
        {
            CacheUnderTest.IsKnownASType(FakeTypeForASTypeInfoCacheTestsEntity.FakeType).Should().BeTrue();
        }
    }

    public class RegisterAssemblyShould : ASTypeInfoCacheTests
    {
        public RegisterAssemblyShould() => CacheUnderTest.RegisterAssembly(typeof(ASObjectEntity).Assembly);

        [Fact]
        public void IncludeThatAssembly()
        {
            CacheUnderTest.IsKnownASType(ASObjectEntity.ObjectType).Should().BeTrue();
        }

        [Fact]
        public void NotIncludeOtherAssemblies()
        {
            CacheUnderTest.IsKnownASType(FakeTypeForASTypeInfoCacheTestsEntity.FakeType).Should().BeFalse();
        }
    }

    public class IsKnownASTypeShould : ASTypeInfoCacheTests
    {
        [Fact]
        public void ReturnTrue_ForKnownType()
        {
            CacheUnderTest.RegisterAllAssemblies();
            CacheUnderTest.IsKnownASType(ASObjectEntity.ObjectType).Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_ForUnknownType()
        {
            CacheUnderTest.RegisterAllAssemblies();
            CacheUnderTest.IsKnownASType("FakeTypeThatDoesn'tExist").Should().BeFalse();
        }

        [Fact]
        public void ReturnFalse_ForKnownTypeInUnregisteredAssembly()
        {
            CacheUnderTest.RegisterAssembly(typeof(ASObjectEntity).Assembly);
            CacheUnderTest.IsKnownASType(FakeTypeForASTypeInfoCacheTestsEntity.FakeType).Should().BeFalse();
        }
    }

    public class IsASLinkTypeShould : ASTypeInfoCacheTests
    {
        [Fact]
        public void ReturnTrue_ForKnownLink()
        {
            CacheUnderTest.RegisterAllAssemblies();
            CacheUnderTest.IsKnownASType(ASLinkEntity.LinkType).Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_ForUnknownLink()
        {
            CacheUnderTest.RegisterAllAssemblies();
            CacheUnderTest.IsKnownASType("FakeTypeThatDoesn'tExist").Should().BeFalse();
        }

        [Fact]
        public void ReturnFalse_ForKnownLinkInUnregisteredAssembly()
        {
            CacheUnderTest.RegisterAssembly(typeof(ASObjectEntity).Assembly);
            CacheUnderTest.IsKnownASType(FakeTypeForASTypeInfoCacheTestsEntity.FakeType).Should().BeFalse();
        }

        [Fact]
        public void ReturnFalse_ForKnownTypeThatIsNotALink()
        {
            CacheUnderTest.RegisterAllAssemblies();
            CacheUnderTest.IsKnownASType(ASObjectEntity.ObjectType).Should().BeTrue();
        }
    }
}

[APConvertible(FakeType)]
public class FakeTypeForASTypeInfoCacheTestsEntity : ASEntity<FakeTypeForASTypeInfoCacheTests>, ILinkEntity
{
    public const string FakeType = "FakeTypeForASTypeInfoCache";
    public override string ASTypeName => FakeType;
    public bool RequiresObjectForm => false;
}

public class FakeTypeForASTypeInfoCacheTests : ASLink
{
    public FakeTypeForASTypeInfoCacheTests() {}
    public FakeTypeForASTypeInfoCacheTests(TypeMap typeMap) : base(typeMap) {}
}