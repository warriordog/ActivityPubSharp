using ActivityPub.Types.AS;
using ActivityPub.Types.AS.Collection;
using ActivityPub.Types.Tests.Util.Fixtures;

// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public abstract class CollectionDeserializationTests : DeserializationTests<ASObject>
{
    private CollectionDeserializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}

    public class ItemsShould(JsonLdSerializerFixture fixture)
        : CollectionDeserializationTests(fixture)
    {

        [Fact]
        public void PopulateInCollection()
        {
            JsonUnderTest = """{"type":"Collection","totalItems":1,"items":[{}]}""";

            ObjectUnderTest.Is<ASCollection>().Should().BeTrue();
            ObjectUnderTest.As<ASCollection>().HasItems.Should().BeTrue();
            ObjectUnderTest.As<ASCollection>().Items.Should().NotBeNull();
            ObjectUnderTest.As<ASCollection>().Items.Should().HaveCount(1);
            ObjectUnderTest.As<ASCollection>().Should().HaveCount(1);
        }

        [Fact]
        public void PopulateInOrderedCollection()
        {
            JsonUnderTest = """{"type":"OrderedCollection","totalItems":1,"orderedItems":[{}]}""";

            ObjectUnderTest.Is<ASOrderedCollection>().Should().BeTrue();
            ObjectUnderTest.As<ASOrderedCollection>().HasItems.Should().BeTrue();
            ObjectUnderTest.As<ASOrderedCollection>().Items.Should().NotBeNull();
            ObjectUnderTest.As<ASOrderedCollection>().Items.Should().HaveCount(1);
            ObjectUnderTest.As<ASOrderedCollection>().Should().HaveCount(1);
        }

        [Fact]
        public void PopulateInCollectionPage()
        {
            JsonUnderTest = """{"type":"CollectionPage","totalItems":1,"items":[{}]}""";

            ObjectUnderTest.Is<ASCollectionPage>().Should().BeTrue();
            ObjectUnderTest.As<ASCollectionPage>().HasItems.Should().BeTrue();
            ObjectUnderTest.As<ASCollectionPage>().Items.Should().NotBeNull();
            ObjectUnderTest.As<ASCollectionPage>().Items.Should().HaveCount(1);
            ObjectUnderTest.As<ASCollectionPage>().Should().HaveCount(1);
        }

        [Fact]
        public void PopulateInOrderedCollectionPage()
        {
            JsonUnderTest = """{"type":"OrderedCollectionPage","totalItems":1,"orderedItems":[{}]}""";

            ObjectUnderTest.Is<ASOrderedCollectionPage>().Should().BeTrue();
            ObjectUnderTest.As<ASOrderedCollectionPage>().HasItems.Should().BeTrue();
            ObjectUnderTest.As<ASOrderedCollectionPage>().Items.Should().NotBeNull();
            ObjectUnderTest.As<ASOrderedCollectionPage>().Items.Should().HaveCount(1);
            ObjectUnderTest.As<ASOrderedCollectionPage>().Should().HaveCount(1);
        }
    }

    public class LinkElementTests(JsonLdSerializerFixture fixture)
        : CollectionDeserializationTests(fixture)
    {

        [Fact]
        public void PopulateLinkElements()
        {
            JsonUnderTest = """{"type":"OrderedCollection","totalItems":1,"orderedItems":[{"type":"Link","href":"https://example.com"}]}""";
            var obj = ObjectUnderTest.As<ASOrderedCollection>();
            obj.TotalItems.Should().Be(1);
            obj.Should().HaveCount(1);
            obj.Items?.First().HasLink.Should().BeTrue();
            obj.Items?.First().Link?.HRef.Should().Be("https://example.com");
        }


        [Fact]
        public void PopulateLinkElements_WhenInStringForm()
        {
            JsonUnderTest = """{"type":"OrderedCollection","totalItems":1,"orderedItems":["https://example.com"]}""";
            var obj = ObjectUnderTest.As<ASOrderedCollection>();
            obj.TotalItems.Should().Be(1);
            obj.Should().HaveCount(1);
            obj.Items?.First().HasLink.Should().BeTrue();
            obj.Items?.First().Link?.HRef.Should().Be("https://example.com");
        }


        [Fact]
        public void PopulateMixedLists()
        {
            JsonUnderTest = """{"type":"OrderedCollection","totalItems":2,"orderedItems":[{},{"type":"Link","href":"https://example.com/1"},"https://example.com/2"]}""";
            var obj = ObjectUnderTest.As<ASOrderedCollection>();
            obj.TotalItems.Should().Be(2);
            obj.Should().HaveCount(3);
            obj.Items?[0].HasValue.Should().BeTrue();
            obj.Items?[1].HasLink.Should().BeTrue();
            obj.Items?[2].HasLink.Should().BeTrue();
        }
    }
}