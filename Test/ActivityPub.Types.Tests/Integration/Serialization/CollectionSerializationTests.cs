// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.AS.Collection;
using ActivityPub.Types.Tests.Util.Fixtures;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public abstract class CollectionSerializationTests : SerializationTests
{
    public class ASCollectionSerializationTests : CollectionSerializationTests
    {
        [Fact]
        public void EmptyCollectionShould_SerializeToObject()
        {
            ObjectUnderTest = new ASCollection();
            JsonUnderTest.Should().BeJsonObject();
        }

        [Fact]
        public void PopulatedCollectionShould_SerializeToObject()
        {
            ObjectUnderTest = new ASCollection
            {
                Items = new LinkableList<ASObject>
                {
                    new ASObject()
                }
            };
            JsonUnderTest.Should().BeJsonObject();
        }

        [Fact]
        public void CollectionOfLinksShould_SerializeToObject()
        {
            ObjectUnderTest = new ASCollection
            {
                Items = new LinkableList<ASObject>
                {
                    "https://home.example/item/whatever",
                    "https://home.example/item/whatever-2"
                }
            };
            JsonUnderTest.Should().BeJsonObject();
        }

        [Fact]
        public void NestedCollectionShould_SerializeToObject()
        {
            var collection = new ASCollection
            {
                Items = new LinkableList<ASObject>
                {
                    "https://home.example/item/whatever",
                    "https://home.example/item/whatever-2"
                }
            };
            ObjectUnderTest = new ASActivity
            {
                Target = [collection]
            };
            JsonUnderTest.Should().BeJsonObject().And.HaveProperty("target");
            JsonUnderTest.GetProperty("target").Should().BeJsonObject();
        }
        
        public ASCollectionSerializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    public class ASOrderedCollectionSerializationTests : CollectionSerializationTests
    {

        [Fact]
        public void EmptyCollectionShould_SerializeToObject()
        {
            ObjectUnderTest = new ASOrderedCollection();
            JsonUnderTest.Should().BeJsonObject();
        }

        [Fact]
        public void PopulatedCollectionShould_SerializeToObject()
        {
            ObjectUnderTest = new ASOrderedCollection
            {
                Items = new LinkableList<ASObject>
                {
                    new ASObject()
                }
            };
            JsonUnderTest.Should().BeJsonObject();
        }

        [Fact]
        public void CollectionOfLinksShould_SerializeToObject()
        {
            ObjectUnderTest = new ASOrderedCollection
            {
                Items = new LinkableList<ASObject>
                {
                    "https://home.example/item/whatever",
                    "https://home.example/item/whatever-2"
                }
            };
            JsonUnderTest.Should().BeJsonObject();
        }

        [Fact]
        public void NestedCollectionShould_SerializeToObject()
        {
            var collection = new ASOrderedCollection
            {
                Items = new LinkableList<ASObject>
                {
                    "https://home.example/item/whatever",
                    "https://home.example/item/whatever-2"
                }
            };
            ObjectUnderTest = new ASActivity
            {
                Target = [collection]
            };
            JsonUnderTest.Should().BeJsonObject().And.HaveProperty("target");
            JsonUnderTest.GetProperty("target").Should().BeJsonObject();
        }
        
        public ASOrderedCollectionSerializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    public class ASCollectionPageSerializationTests : CollectionSerializationTests
    {

        [Fact]
        public void EmptyCollectionShould_SerializeToObject()
        {
            ObjectUnderTest = new ASCollectionPage();
            JsonUnderTest.Should().BeJsonObject();
        }

        [Fact]
        public void PopulatedCollectionShould_SerializeToObject()
        {
            ObjectUnderTest = new ASCollectionPage
            {
                Items = new LinkableList<ASObject>
                {
                    new ASObject()
                }
            };
            JsonUnderTest.Should().BeJsonObject();
        }

        [Fact]
        public void CollectionOfLinksShould_SerializeToObject()
        {
            ObjectUnderTest = new ASCollectionPage
            {
                Items = new LinkableList<ASObject>
                {
                    "https://home.example/item/whatever",
                    "https://home.example/item/whatever-2"
                }
            };
            JsonUnderTest.Should().BeJsonObject();
        }

        [Fact]
        public void NestedCollectionShould_SerializeToObject()
        {
            var collection = new ASCollectionPage
            {
                Items = new LinkableList<ASObject>
                {
                    "https://home.example/item/whatever",
                    "https://home.example/item/whatever-2"
                }
            };
            ObjectUnderTest = new ASActivity
            {
                Target = [collection]
            };
            JsonUnderTest.Should().BeJsonObject().And.HaveProperty("target");
            JsonUnderTest.GetProperty("target").Should().BeJsonObject();
        }
        
        public ASCollectionPageSerializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    public class ASOrderedCollectionPageSerializationTests : CollectionSerializationTests
    {

        [Fact]
        public void EmptyCollectionShould_SerializeToObject()
        {
            ObjectUnderTest = new ASOrderedCollectionPage();
            JsonUnderTest.Should().BeJsonObject();
        }

        [Fact]
        public void PopulatedCollectionShould_SerializeToObject()
        {
            ObjectUnderTest = new ASOrderedCollectionPage
            {
                Items = new LinkableList<ASObject>
                {
                    new ASObject()
                }
            };
            JsonUnderTest.Should().BeJsonObject();
        }

        [Fact]
        public void CollectionOfLinksShould_SerializeToObject()
        {
            ObjectUnderTest = new ASOrderedCollectionPage
            {
                Items = new LinkableList<ASObject>
                {
                    "https://home.example/item/whatever",
                    "https://home.example/item/whatever-2"
                }
            };
            JsonUnderTest.Should().BeJsonObject();
        }

        [Fact]
        public void NestedCollectionShould_SerializeToObject()
        {
            var collection = new ASOrderedCollectionPage
            {
                Items = new LinkableList<ASObject>
                {
                    "https://home.example/item/whatever",
                    "https://home.example/item/whatever-2"
                }
            };
            ObjectUnderTest = new ASActivity
            {
                Target = [collection]
            };
            JsonUnderTest.Should().BeJsonObject().And.HaveProperty("target");
            JsonUnderTest.GetProperty("target").Should().BeJsonObject();
        }
        
        public ASOrderedCollectionPageSerializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    protected CollectionSerializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}
}