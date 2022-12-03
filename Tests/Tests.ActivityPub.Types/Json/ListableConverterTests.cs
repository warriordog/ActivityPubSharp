using ActivityPub.Types.Json;
using Newtonsoft.Json;

namespace Tests.ActivityPub.Types.Json;

public class ListableConverterTests
{
    public class WriteJsonShould : ListableConverterTests
    {
        public static TheoryData<ICollection<string>> SingleElementData => new()
        {
            new List<string> { "single" },
            new HashSet<string> { "single" },
            new[] { "single" }
        };

        [Theory]
        [MemberData(nameof(SingleElementData))]
        public void FlattenCollections_WhenThereIsOnlyOneElement(ICollection<string> collection)
        {
            var testObject = new TestJsonEntity { TestProp = collection };
            var json = JsonConvert.SerializeObject(testObject);
            json.Should().Be("{\"TestProp\":\"single\"}");
        }

        public static TheoryData<ICollection<string>> MultiElementData => new()
        {
            new List<string> { "multi1", "multi2", "multi3" },
            new HashSet<string> { "multi1", "multi2", "multi3" },
            new[] { "multi1", "multi2", "multi3" }
        };
        
        [Theory]
        [MemberData(nameof(MultiElementData))]
        public void PreserveCollections_WhenThereIsMoreThanOneElement(ICollection<string> collection)
        {
            var testObject = new TestJsonEntity { TestProp = collection };
            var json = JsonConvert.SerializeObject(testObject);
            json.Should().Be("{\"TestProp\":[\"multi1\",\"multi2\",\"multi3\"]}");
        }

        public static TheoryData<ICollection<string>> NoElementData => new()
        {
            new List<string>(),
            new HashSet<string>(),
            Array.Empty<string>()
        };
        
        [Theory]
        [MemberData(nameof(NoElementData))]
        public void PreserveCollections_WhenThereAreNoElements(ICollection<string> collection)
        {
            var testObject = new TestJsonEntity { TestProp = collection };
            var json = JsonConvert.SerializeObject(testObject);
            json.Should().Be("{\"TestProp\":[]}");
        }

        [JsonObject]
        internal class TestJsonEntity
        {
            [JsonProperty]
            [JsonConverter(typeof(ListableConverter<string>))]
            public ICollection<string>? TestProp { get; set; }
        }
    }

    public class ReadJsonShould : ListableConverterTests
    {
        [Fact]
        public void IncludeAllElements_WhenInputIsAnArray()
        {
            var json = "{\"TestList\":[\"multi1\",\"multi2\",\"multi3\"],\"TestSet\":[\"multi1\",\"multi2\",\"multi3\"]}";
            var testObject = JsonConvert.DeserializeObject<TestJsonEntity>(json);
            testObject.Should().NotBeNull();
            testObject!.TestList.Should().NotBeNull();
            testObject.TestList.Should().BeEquivalentTo("multi1", "multi2", "multi3");
            testObject.TestSet.Should().NotBeNull();
            testObject.TestSet.Should().BeEquivalentTo("multi1", "multi2", "multi3");
        }

        [Fact]
        public void ExpandIntoCollection_WhenInputIsAnObject()
        {
            var json = "{\"TestList\":\"single\",\"TestSet\":\"single\"}";
            var testObject = JsonConvert.DeserializeObject<TestJsonEntity>(json);
            testObject.Should().NotBeNull();
            testObject!.TestList.Should().NotBeNull();
            testObject.TestList.Should().BeEquivalentTo("single");
            testObject.TestSet.Should().NotBeNull();
            testObject.TestSet.Should().BeEquivalentTo("single");
        }

        [Fact]
        public void PreserveExistingCollection_WhenPropertyHasExistingValue()
        {
            var json = "{\"TestList\":[\"multi2\",\"multi3\"],\"TestSet\":[\"multi2\",\"multi3\"]}";
            var list = new List<string> { "multi1" };
            var set = new HashSet<string> { "multi1" };
            
            var testObject = new TestJsonEntity { TestList = list, TestSet = set};
            JsonConvert.PopulateObject(json, testObject);
            
            testObject.TestList.Should().BeSameAs(list);
            list.Should().BeEquivalentTo("multi1", "multi2", "multi3");
            
            testObject.TestSet.Should().BeSameAs(set);
            set.Should().BeEquivalentTo("multi1", "multi2", "multi3");
        }

        [Fact]
        public void CreateNewCollection_WhenPropertyIsNull()
        {
            var json = "{\"TestList\":\"single\",\"TestSet\":\"single\"}";
            
            var testObject = new TestJsonEntity { TestList = null, TestSet = null };
            JsonConvert.PopulateObject(json, testObject);

            testObject.TestList.Should().BeEquivalentTo("single");
            testObject.TestSet.Should().BeEquivalentTo("single");
        }

        [JsonObject]
        internal class TestJsonEntity
        {
            [JsonProperty]
            [JsonConverter(typeof(ListableConverter<string>))]
            public List<string>? TestList { get; set; }

            [JsonProperty]
            [JsonConverter(typeof(ListableConverter<string>))]
            public HashSet<string>? TestSet { get; set; }
        }
    }
}