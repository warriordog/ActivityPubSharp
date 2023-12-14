// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using ActivityPub.Types.AS;
using ActivityPub.Types.Conversion;
using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Tests.Util.Fakes;
using ActivityPub.Types.Util;

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
            CacheUnderTest.TryGetModelType(ASObject.ObjectType, out _).Should().BeTrue();
        }

        [Fact]
        public void IncludeOtherPackages()
        {
            CacheUnderTest.TryGetModelType(FakeTypeForASTypeInfoCacheTests.FakeTypeForASTypeInfoCacheTestsType, out _).Should().BeTrue();
        }
    }

    public class RegisterAssemblyShould : ASTypeInfoCacheTests
    {
        public RegisterAssemblyShould() => CacheUnderTest.RegisterAssembly(typeof(ASObjectEntity).Assembly);

        [Fact]
        public void IncludeThatAssembly()
        {
            CacheUnderTest.TryGetModelType(ASObject.ObjectType, out _).Should().BeTrue();
        }

        [Fact]
        public void NotIncludeOtherAssemblies()
        {
            CacheUnderTest.TryGetModelType(FakeTypeForASTypeInfoCacheTests.FakeTypeForASTypeInfoCacheTestsType, out _).Should().BeFalse();
        }
    }

    public class AnonymousEntityTypesShould : ASTypeInfoCacheTests
    {
        public AnonymousEntityTypesShould() => CacheUnderTest.RegisterAllAssemblies();

        [Fact]
        public void IncludeAllDetectedAnonymousTypes()
        {
            var expectedType = typeof(AnonymousExtensionFakeEntity);
            CacheUnderTest.AnonymousEntityTypes.Should().Contain(expectedType);
        }

        [Fact]
        public void ExcludeInvalidAnonymousTypes()
        {
            var unexpectedType = typeof(FakeTypeWithInvalidInterfaces);
            CacheUnderTest.AnonymousEntityTypes.Should().NotContain(unexpectedType);
        }
    }

    public class NamelessEntityTypesShould : ASTypeInfoCacheTests
    {
        public NamelessEntityTypesShould() => CacheUnderTest.RegisterAllAssemblies();

        [Fact]
        public void IncludeAllDetectedNamelessTypes()
        {
            var expectedType = typeof(NamelessExtensionFakeEntity);
            CacheUnderTest.NamelessEntityTypes.Should().Contain(expectedType);
        }

        [Fact]
        public void ExcludeInvalidNamelessTypes()
        {
            var unexpectedType = typeof(FakeTypeWithInvalidInterfaces);
            CacheUnderTest.NamelessEntityTypes.Should().NotContain(unexpectedType);
        }
    }
}

public class FakeTypeWithInvalidInterfaces : ASType, IAnonymousEntity, INamelessEntity
{
    public static bool ShouldConvertFrom(JsonElement inputJson, DeserializationMetadata meta) => throw new NotSupportedException();
    public static bool ShouldConvertFrom(IJsonLDContext jsonLDContext, DeserializationMetadata meta) => throw new NotSupportedException();
}

public class FakeTypeForASTypeInfoCacheTests : ASLink, IASModel<FakeTypeForASTypeInfoCacheTests, FakeTypeForASTypeInfoCacheTestsEntity, ASLink>
{
    /// <summary>
    ///     ActivityStreams type name for "FakeTypeForASTypeInfoCacheTests" types.
    /// </summary>
    public const string FakeTypeForASTypeInfoCacheTestsType = "FakeTypeForASTypeInfoCacheTests";
    static string IASModel<FakeTypeForASTypeInfoCacheTests>.ASTypeName => FakeTypeForASTypeInfoCacheTestsType;

    /// <inheritdoc />
    public FakeTypeForASTypeInfoCacheTests() => Entity = TypeMap.Extend<FakeTypeForASTypeInfoCacheTestsEntity>();

    /// <inheritdoc />
    public FakeTypeForASTypeInfoCacheTests(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<FakeTypeForASTypeInfoCacheTestsEntity>(isExtending);

    /// <inheritdoc />
    public FakeTypeForASTypeInfoCacheTests(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public FakeTypeForASTypeInfoCacheTests(TypeMap typeMap, FakeTypeForASTypeInfoCacheTestsEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<FakeTypeForASTypeInfoCacheTestsEntity>();

    static FakeTypeForASTypeInfoCacheTests IASModel<FakeTypeForASTypeInfoCacheTests>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private FakeTypeForASTypeInfoCacheTestsEntity Entity { get; }
}

public class FakeTypeForASTypeInfoCacheTestsEntity : ASEntity<FakeTypeForASTypeInfoCacheTests, FakeTypeForASTypeInfoCacheTestsEntity> {}