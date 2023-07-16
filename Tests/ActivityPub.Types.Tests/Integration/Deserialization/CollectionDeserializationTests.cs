using ActivityPub.Types.Collection;
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public class CollectionDeserializationTests : DeserializationTests<ASObject>
{
    public class ItemsShould : CollectionDeserializationTests
    {
        [Fact]
        public void PopulateInCollection()
        {
            JsonUnderTest = """{"type":"Collection","totalItems":1,"items":[{}]}""";
            
            ObjectUnderTest.Should().BeOfType<ASCollection<ASType>>();
            ObjectUnderTest.As<ASCollection<ASType>>().HasItems.Should().BeTrue();
            ObjectUnderTest.As<ASCollection<ASType>>().Items.Should().NotBeNull();
            ObjectUnderTest.As<ASCollection<ASType>>().Items.Should().HaveCount(1);
        }

        [Fact]
        public void PopulateInOrderedCollection()
        {
            JsonUnderTest = """{"type":"OrderedCollection","totalItems":1,"orderedItems":[{}]}""";
            
            ObjectUnderTest.Should().BeOfType<ASOrderedCollection<ASType>>();
            ObjectUnderTest.As<ASOrderedCollection<ASType>>().HasItems.Should().BeTrue();
            ObjectUnderTest.As<ASOrderedCollection<ASType>>().Items.Should().NotBeNull();
            ObjectUnderTest.As<ASOrderedCollection<ASType>>().Items.Should().HaveCount(1);
        }
        
        [Fact]
        public void PopulateInCollectionPage()
        {
            JsonUnderTest = """{"type":"CollectionPage","totalItems":1,"items":[{}]}""";
            
            ObjectUnderTest.Should().BeOfType<ASCollectionPage<ASType>>();
            ObjectUnderTest.As<ASCollectionPage<ASType>>().HasItems.Should().BeTrue();
            ObjectUnderTest.As<ASCollectionPage<ASType>>().Items.Should().NotBeNull();
            ObjectUnderTest.As<ASCollectionPage<ASType>>().Items.Should().HaveCount(1);
        }

        [Fact]
        public void PopulateInOrderedCollectionPage()
        {
            JsonUnderTest = """{"type":"OrderedCollectionPage","totalItems":1,"orderedItems":[{}]}""";
            
            ObjectUnderTest.Should().BeOfType<ASOrderedCollectionPage<ASType>>();
            ObjectUnderTest.As<ASOrderedCollectionPage<ASType>>().HasItems.Should().BeTrue();
            ObjectUnderTest.As<ASOrderedCollectionPage<ASType>>().Items.Should().NotBeNull();
            ObjectUnderTest.As<ASOrderedCollectionPage<ASType>>().Items.Should().HaveCount(1);
        }
    }
}