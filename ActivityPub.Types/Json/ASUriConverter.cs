using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Json;

public class ASUriConverter : JsonConverter<ASUri>
{
    public override ASUri Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Read the URI in string format
        if (!reader.TryGetString(out var uriString))
            throw new JsonException($"Failed to convert ASUri - could not read a string from JSON input.");
        
        // Create it - mmm yes very simple yes
        return new ASUri(uriString);
    }
    
    // Nice and simple
    public override void Write(Utf8JsonWriter writer, ASUri value, JsonSerializerOptions options)
    {
        var uriString = value.ToString();
        
        writer.WriteStringValue(uriString);
    }
}