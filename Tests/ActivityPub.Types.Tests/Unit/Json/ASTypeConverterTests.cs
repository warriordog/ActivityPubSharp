// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Json;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Unit.Json;

public abstract class ASTypeConverterTests : JsonConverterTests<ASType, ASTypeConverter<ASType>>, IClassFixture<JsonLdSerializerFixture>
{
    protected override sealed ASTypeConverter<ASType> ConverterUnderTest { get; set; }

    private ASTypeConverterTests(JsonLdSerializerFixture fixture)
    {
        ConverterUnderTest = new(fixture.ASTypeInfoCache, fixture.JsonTypeInfoCache);
        JsonSerializerOptions = fixture.JsonLdSerializer.SerializerOptions;
    }

    public class ReadShould : ASTypeConverterTests
    {
        public ReadShould(JsonLdSerializerFixture fixture) : base(fixture) {}
        
        [Fact]
        public void DeserializeToObject_WhenTypeIsUnknownAndBaseIsASType()
        {
            var json = """{"type":"MadeUpType"}"""u8;
            var obj = Read(json);
            obj.Should().BeOfType<ASObject>();
        }
    }

    public class WriteShould : ASTypeConverterTests
    {
        public WriteShould(JsonLdSerializerFixture fixture) : base(fixture) {}


        [Fact]
        public void ExcludeContextFromNestedObject()
        {
            var obj = new ASObject
            {
                Source = new ASObject()
            };
            
            var element = WriteToElement(obj);

            element.Should().HaveStringProperty("@context", "https://www.w3.org/ns/activitystreams");
            element.Should().HaveProperty("source", source =>
                source.Should().NotHaveProperty("@context")
            );
        }
    }
}