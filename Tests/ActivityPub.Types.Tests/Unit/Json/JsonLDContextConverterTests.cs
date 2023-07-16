﻿// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Json;

public class JsonLDContextConverterTests : JsonConverterTests<JsonLDContext, JsonLDContextConverter>
{
    protected override JsonLDContextConverter ConverterUnderTest { get; set; } = new();

    public class ReadShould : JsonLDContextConverterTests
    {

        [Fact]
        public void PassThroughNull()
        {
            var json = "null"u8;
            var result = Read(json);
            result.Should().BeNull();
        }

        [Fact]
        public void ParseStringAsContext()
        {
            var json = "\"https://example.com/context.jsonld\""u8;
            
            var result = Read(json);
            
            result.Should().NotBeNull();
            result?.ContextObjects.Should().HaveCount(1);
            result?.ContextObjects.First().IsExternal.Should().BeTrue();
            result?.ContextObjects.First().ExternalLink?.Should().Be("https://example.com/context.jsonld");
        }

        [Fact]
        public void ParseObjectAsContext()
        {
            var json = "{\"name\":\"https://example.com/context.jsonld\"}"u8;
            
            var result = Read(json);
            
            result.Should().NotBeNull();
            result?.ContextObjects.Should().HaveCount(1);
            result?.ContextObjects.First().IsEmbedded.Should().BeTrue();
        }

        [Fact]
        public void DeserializeArrayOfStrings()
        {
            var json = "[\"https://example.com/first.jsonld\",\"https://example.com/second.jsonld\"]"u8;
            
            var result = Read(json);
            
            result.Should().NotBeNull();
            result?.ContextObjects.Should().HaveCount(2);
        }

        [Fact]
        public void DeserializeArrayOfObjects()
        {
            var json = "[{\"name\":\"https://example.com/name_first\"},{\"name\":\"https://example.com/name_second\"}]"u8;
            
            var result = Read(json);
            
            result.Should().NotBeNull();
            result?.ContextObjects.Should().HaveCount(2);
        }

        [Fact]
        public void DeserializeArrayOfStringsAndObjects()
        {
            var json = "[\"https://example.com/first.jsonld\",{\"name\":\"https://example.com/name_second\"}]"u8;
            
            var result = Read(json);
            
            result.Should().NotBeNull();
            result?.ContextObjects.Should().HaveCount(2);
        }
        
        [Fact]
        public void ThrowJsonException_WhenInputIsUnsupported()
        {
            Assert.Throws<JsonException>(() =>
            {
                var json = "0"u8;
                Read(json);
            });
        }
    }

    public class WriteShould : JsonLDContextConverterTests
    {
        [Fact]
        public void WriteSingleDirectly()
        {
            var input = new JsonLDContext(new HashSet<JsonLDContextObject>()
            {
                new JsonLDContextObject("https://example.com/context.jsonld")
            });
            
            var json = Write(input);

            json.Should().Be("\"https://example.com/context.jsonld\"");
        }

        [Fact]
        public void WriteMultiAsArray()
        {
            var input = new JsonLDContext(new HashSet<JsonLDContextObject>
            {
                new JsonLDContextObject("https://example.com/first.jsonld"),
                new JsonLDContextObject("https://example.com/second.jsonld")
            });
            
            var json = Write(input);

            json.Should().Be("[\"https://example.com/first.jsonld\",\"https://example.com/second.jsonld\"]");
        }
    }
}