using System.Text;
using System.Text.Json.Serialization;

namespace ActivityPub.Types.Tests.Unit.Json;

public abstract class JsonConverterTests<T, TConverter>
where TConverter : JsonConverter<T>
{
    protected abstract TConverter ConverterUnderTest { get; set; }

    protected T? Read(ReadOnlySpan<byte> json)
    {
        var reader = new Utf8JsonReader(json);
        reader.Read();
            
        return ConverterUnderTest.Read(ref reader, typeof(T), JsonSerializerOptions.Default);
    }
    
    protected string Write(T input)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        
        ConverterUnderTest.Write(writer, input, JsonSerializerOptions.Default);
        
        writer.Flush();
        return Encoding.UTF8.GetString(stream.ToArray());
    }
}