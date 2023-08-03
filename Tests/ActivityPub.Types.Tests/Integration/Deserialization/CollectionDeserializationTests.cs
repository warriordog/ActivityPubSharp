using ActivityPub.Types.Collection;
using ActivityPub.Types.Tests.Util.Fixtures;

// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public abstract class CollectionDeserializationTests : DeserializationTests<ASObject>
{
    public class ItemsShould : CollectionDeserializationTests
    {
        [Fact]
        public void PopulateInCollection()
        {
            JsonUnderTest = """{"type":"Collection","totalItems":1,"items":[{}]}""";

            ObjectUnderTest.Should().BeOfType<ASCollection<ASObject>>();
            ObjectUnderTest.As<ASCollection<ASObject>>().HasItems.Should().BeTrue();
            ObjectUnderTest.As<ASCollection<ASObject>>().Items.Should().NotBeNull();
            ObjectUnderTest.As<ASCollection<ASObject>>().Items.Should().HaveCount(1);
        }

        [Fact]
        public void PopulateInOrderedCollection()
        {
            JsonUnderTest = """{"type":"OrderedCollection","totalItems":1,"orderedItems":[{}]}""";

            ObjectUnderTest.Should().BeOfType<ASCollection<ASObject>>();
            ObjectUnderTest.As<ASCollection<ASObject>>().HasItems.Should().BeTrue();
            ObjectUnderTest.As<ASCollection<ASObject>>().Items.Should().NotBeNull();
            ObjectUnderTest.As<ASCollection<ASObject>>().Items.Should().HaveCount(1);
        }

        [Fact]
        public void PopulateInCollectionPage()
        {
            JsonUnderTest = """{"type":"CollectionPage","totalItems":1,"items":[{}]}""";

            ObjectUnderTest.Should().BeOfType<ASCollectionPage<ASObject>>();
            ObjectUnderTest.As<ASCollectionPage<ASObject>>().HasItems.Should().BeTrue();
            ObjectUnderTest.As<ASCollectionPage<ASObject>>().Items.Should().NotBeNull();
            ObjectUnderTest.As<ASCollectionPage<ASObject>>().Items.Should().HaveCount(1);
        }

        [Fact]
        public void PopulateInOrderedCollectionPage()
        {
            JsonUnderTest = """{"type":"OrderedCollectionPage","totalItems":1,"orderedItems":[{}]}""";

            ObjectUnderTest.Should().BeOfType<ASCollectionPage<ASObject>>();
            ObjectUnderTest.As<ASCollectionPage<ASObject>>().HasItems.Should().BeTrue();
            ObjectUnderTest.As<ASCollectionPage<ASObject>>().Items.Should().NotBeNull();
            ObjectUnderTest.As<ASCollectionPage<ASObject>>().Items.Should().HaveCount(1);
        }

        public ItemsShould(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    public class LinkElementTests : CollectionDeserializationTests
    {
        [Fact]
        public void PopulateLinkElements()
        {
            JsonUnderTest = """{"type":"OrderedCollection","totalItems":1,"orderedItems":[{"type":"Link","href":"https://example.com"}]}""";
            var obj = ObjectUnderTest.As<ASCollection<ASObject>>();
            obj.TotalItems.Should().Be(1);
            obj.Items?.First().HasLink.Should().BeTrue();
            obj.Items?.First().Link?.HRef.Should().Be("https://example.com");
        }


        [Fact]
        public void PopulateLinkElements_WhenInStringForm()
        {
            JsonUnderTest = """{"type":"OrderedCollection","totalItems":1,"orderedItems":["https://example.com"]}""";
            var obj = ObjectUnderTest.As<ASCollection<ASObject>>();
            obj.TotalItems.Should().Be(1);
            obj.Items?.First().HasLink.Should().BeTrue();
            obj.Items?.First().Link?.HRef.Should().Be("https://example.com");
        }


        [Fact]
        public void PopulateMixedLists()
        {
            JsonUnderTest = """{"type":"OrderedCollection","totalItems":2,"orderedItems":[{},{"type":"Link","href":"https://example.com/1"},"https://example.com/2"]}""";
            var obj = ObjectUnderTest.As<ASCollection<ASObject>>();
            obj.TotalItems.Should().Be(2);
            obj.Items?[0].HasValue.Should().BeTrue();
            obj.Items?[1].HasLink.Should().BeTrue();
            obj.Items?[2].HasLink.Should().BeTrue();
        }

        public LinkElementTests(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    private CollectionDeserializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}
}