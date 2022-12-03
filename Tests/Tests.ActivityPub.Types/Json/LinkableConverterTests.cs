using ActivityPub.Types;
using ActivityPub.Types.Util;
using Newtonsoft.Json;

namespace Tests.ActivityPub.Types.Json;

// TODO finish these

public class LinkableConverterTests
{
    public class WriteJsonShould : ListableConverterTests
    {

        public void GenerateLink_WhenLinkableContainsLink()
        {
            var entity = new TestJsonEntity { LinkOrObject = new ASLink { HRef = "https://example.com" } };
            var json = JsonConvert.SerializeObject(entity);
            json.Should().Be("{\"LinkOrObject\":{\"type\":\"Link\",\"href\":\"https://example.com\"}}");
        }

        public void GenerateObject_WhenLinkableDoesNotContainLink()
        {
            
        }
        
        [JsonObject]
        public class TestJsonEntity
        {
            [JsonProperty]
            public Linkable<ASObject>? LinkOrObject { get; set; }
        }
    }

    public class ReadJsonShould : ListableConverterTests
    {
        public void PopulateWithLink_WhenTypeIsLink()
        {
            
        }

        public void PopulateWithLink_WhenTypeIsString()
        {
            
        }

        public void PopulateWithObject_WhenTypeIsNotLink()
        {
            
        }

        public void PopulateWithSpecificType_WhenTypeIsNotASObject()
        {
            
        }
    }
}