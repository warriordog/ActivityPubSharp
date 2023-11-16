// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.AS.Extended.Activity;
using ActivityPub.Types.Tests.Util.Fakes;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests;

public abstract class TypeMapTests
{
    public class ConstructorZeroShould : TypeMapTests
    {
        [Fact]
        public void CreateNewContext()
        {
            var first = new TypeMap().LDContext;
            var second = new TypeMap().LDContext;
            first.Should().NotBeSameAs(second);

        }

        [Fact]
        public void AddActivityStreamsContext()
        {
            var typeMap = new TypeMap();
            typeMap.LDContext.Should().Contain(JsonLDContextObject.ActivityStreams);
        }
    }

    public class ConstructorOneShould : TypeMapTests
    {
        [Fact]
        public void AssignContext()
        {
            var context = JsonLDContext.CreateASContext();
            var typeMap = new TypeMap(context);
            typeMap.LDContext.Should().BeSameAs(context);
        }
    }

    public class TryAddShould : TypeMapTests
    {
        [Fact]
        public void ReturnFalse_IfEntityIsDuplicate()
        {
            var typeMap = new TypeMap();
            var entity = new ASTypeEntity();
            typeMap.AddEntity(entity);

            typeMap.TryAddEntity(entity).Should().BeFalse();
        }

        [Fact]
        public void ReturnFalse_IfEntityTypeIsDuplicate()
        {
            var typeMap = new TypeMap();
            typeMap.AddEntity(new ASTypeEntity());

            var entity = new ASTypeEntity();
            typeMap.TryAddEntity(entity).Should().BeFalse();
        }
        
        [Fact]
        public void DoNothing_IfEntityIsDuplicate()
        {
            var typeMap = new TypeMap();
            var entity = new ASTypeEntity();
            typeMap.AddEntity(entity);

            typeMap.TryAddEntity(entity);

            typeMap.AllEntities.Should().HaveCount(1);
        }

        [Fact]
        public void ReturnTrue_IfEntityIsNotDuplicate()
        {
            var typeMap = new TypeMap();
            var entity = new ASTypeEntity();
            
            typeMap.TryAddEntity(entity).Should().BeTrue();
        }

        [Fact]
        public void AddEntity_IfEntityIsNotDuplicate()
        {
            var typeMap = new TypeMap();
            var entity = new ASTypeEntity();
            
            typeMap.TryAddEntity(entity);
            
            
            typeMap.AllEntities.Should().ContainValue(entity);
        }

        [Fact]
        public void MapEntity_IfItHasASType()
        {
            var typeMap = new TypeMap();
            typeMap.AddEntity(new ASTypeEntity());
         
            typeMap.TryAddEntity(new ASObjectEntity());

            typeMap.ASTypes.Should().Contain(ASObject.ObjectType);
        }

        [Fact]
        public void SkipMapping_IfEntityHasNoASType()
        {
            var typeMap = new TypeMap();
            typeMap.TryAddEntity(new ASTypeEntity());
            typeMap.ASTypes.Should().BeEmpty();
        }

        [Fact]
        public void NarrowUnmappedProperties()
        {
            var typeMap = new TypeMap
            {
                UnmappedProperties = new Dictionary<string, JsonElement>
                {
                    ["1"] = new(),
                    ["2"] = new()
                }
            };
            var entity = new ASTypeEntity
            {
                UnmappedProperties = new Dictionary<string, JsonElement>
                {
                    ["1"] = new()
                }
            };

            typeMap.TryAddEntity(entity);

            typeMap.UnmappedProperties.Should()
                .HaveCount(1).And
                .ContainKey("1");
        }

        [Fact]
        public void MergeContexts()
        {
            const string ExtraContext = "https://example.com/extra/context";
            var originalContext = new JsonLDContext
            {
                JsonLDContextObject.ActivityStreams,
                ExtraContext
            };
            var typeMap = new TypeMap(originalContext);
            
            typeMap.TryAddEntity(new EmptyExtendedTypeFakeEntity());
            
            typeMap.LDContext.Should()
                .HaveCount(3).And
                .Contain(JsonLDContextObject.ActivityStreams).And
                .Contain(ExtraContext).And
                .Contain(EmptyExtendedTypeFake.ExtendedContext);
        }
    }

    public abstract class TypeMapASTypeTests : TypeMapTests
    {
        protected TypeMap TypeMapUnderTest { get; } = new();

        protected TypeMapASTypeTests()
        {
            TypeMapUnderTest.AddEntity(new ASTypeEntity());
            TypeMapUnderTest.AddEntity(new ASObjectEntity());
            TypeMapUnderTest.AddEntity(new ASActivityEntity());
            TypeMapUnderTest.AddEntity(new LikeActivityEntity());
            TypeMapUnderTest.AddEntity(new DislikeActivityEntity());
        }
    }
    public class ASTypesShould : TypeMapASTypeTests
    {
        [Fact]
        public void ContainAllTopLevelTypes()
        {
            TypeMapUnderTest.ASTypes.Should()
                .Contain(LikeActivity.LikeType)
                .And.Contain(DislikeActivity.DislikeType);
        }

        [Fact]
        public void NotContainAnyShadowedTypes()
        {
            TypeMapUnderTest.ASTypes.Should()
                .NotContain(ASObject.ObjectType)
                .And.NotContain(ASActivity.ActivityType);
        }
    }

    public class AllASTypesShould : TypeMapASTypeTests
    {
        [Fact]
        public void ContainAllTopLevelTypes()
        {
            TypeMapUnderTest.AllASTypes.Should()
                .Contain(LikeActivity.LikeType)
                .And.Contain(DislikeActivity.DislikeType);
        }

        [Fact]
        public void ContainAllShadowedTypes()
        {
            TypeMapUnderTest.AllASTypes.Should()
                .Contain(ASObject.ObjectType)
                .And.Contain(ASActivity.ActivityType);
        }
    }
}