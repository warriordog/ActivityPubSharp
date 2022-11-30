using ActivityPub.Common.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ActivityPub.Serialization.Json;

public class ASTypeJsonConverter : JsonConverter
{
    public override bool CanRead => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var obj = JObject.Load(reader);
        
        serializer.
    }

    // Convert any type / subtype of ASType
    public override bool CanConvert(Type objectType) => objectType.IsAssignableTo(typeof(ASType));
    
    // Writing is not supported or needed
    public override bool CanWrite => false;
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => throw new NotImplementedException();

    public static Dictionary<string, Type> CreateDefaultMapping()
    {
        return new Dictionary<string, Type>()
        {
            typeof(ASObject)
        }
    }
}