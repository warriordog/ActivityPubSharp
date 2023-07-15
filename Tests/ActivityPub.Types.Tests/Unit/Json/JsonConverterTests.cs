// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

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