// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.AS.Extended.Activity;
using ActivityPub.Types.Conversion;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Unit.AS.Extended.Activity;

public abstract class QuestionActivityTests : IClassFixture<JsonLdSerializerFixture>
{
    private string JsonUnderTest
    {
        get => _jsonUnderTest.Value;
        set
        {
            _jsonUnderTest = new Lazy<string>(value);
            _questionUnderTest = new Lazy<QuestionActivity>(() => _jsonLdSerializer.Deserialize<QuestionActivity>(value)!);
        }
    }

    private QuestionActivity QuestionUnderTest
    {
        get => _questionUnderTest.Value;
        set
        {
            _jsonUnderTest = new Lazy<string>(() => _jsonLdSerializer.Serialize(value));
            _questionUnderTest = new Lazy<QuestionActivity>(value);
        }
    }

    private Lazy<string> _jsonUnderTest = new(() => throw new InvalidOperationException("Please populate JsonUnderTest or QuestionUnderTest"));
    private Lazy<QuestionActivity> _questionUnderTest = new(() => throw new InvalidOperationException("Please populate JsonUnderTest or QuestionUnderTest"));
    
    private readonly IJsonLdSerializer _jsonLdSerializer;
    protected QuestionActivityTests(JsonLdSerializerFixture fixture) => _jsonLdSerializer = fixture.JsonLdSerializer;

    public class OptionsShould : QuestionActivityTests
    {
        [Fact]
        public void SerializeToOneOf_WhenAllowMultipleIsFalse()
        {
            QuestionUnderTest = new QuestionActivity
            {
                AllowMultiple = false,
                Options = new ASObject()
            };
            JsonUnderTest.Should().Contain("\"oneOf\":");
            JsonUnderTest.Should().NotContain("\"anyOf\":");
        }

        [Fact]
        public void SerializeToAnyOf_WhenAllowMultipleIsTrue()
        {
            QuestionUnderTest = new QuestionActivity
            {
                AllowMultiple = true,
                Options = new ASObject()
            };
            JsonUnderTest.Should().NotContain("\"oneOf\":");
            JsonUnderTest.Should().Contain("\"anyOf\":");
        }

        [Fact]
        public void DeserializeFromOneOf()
        {
            JsonUnderTest = """{"type":"Question","oneOf":{}}""";
            QuestionUnderTest.Options.Should()
                .NotBeNull()
                .And.HaveCount(1);
        }

        [Fact]
        public void DeserializeFromAnyOf()
        {
            JsonUnderTest = """{"type":"Question","anyOf":{}}""";
            QuestionUnderTest.Options.Should()
                .NotBeNull()
                .And.HaveCount(1);
        }

        [Fact]
        public void DeserializeFromOneOfAndAnyOf()
        {
            JsonUnderTest = """{"type":"Question","oneOf":{},"anyOf":{}}""";
            QuestionUnderTest.Options.Should()
                .NotBeNull()
                .And.HaveCount(2);
        }

        [Fact]
        public void NotSerializeToOptions()
        {
            QuestionUnderTest = new QuestionActivity
            {
                Options = new ASObject()
            };
            JsonUnderTest.ToLower().Should().NotContain("\"options\"");
        }

        [Fact]
        public void NotDeserializeFromOptions()
        {
            JsonUnderTest = """{"type":"Question","options":{}}""";
            QuestionUnderTest.Options.Should().BeNull();
        }
        
        public OptionsShould(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    public class AllowMultipleShould : QuestionActivityTests
    {
        [Fact]
        public void DeserializeToFalse_WhenOnOfIsPresent()
        {
            JsonUnderTest = """{"type":"Question","oneOf":{}}""";
            QuestionUnderTest.AllowMultiple.Should().BeFalse();
        }

        [Fact]
        public void DeserializeToTrue_WhenAnyOfIsPresent()
        {
            JsonUnderTest = """{"type":"Question","anyOf":{}}""";
            QuestionUnderTest.AllowMultiple.Should().BeTrue();
        }

        [Fact]
        public void DeserializeToTrue_WhenOneOfAndAnyOfArePresent()
        {
         
            JsonUnderTest = """{"type":"Question","anyOf":{},"oneOf":{}}""";
            QuestionUnderTest.AllowMultiple.Should().BeTrue();   
        }
        
        public AllowMultipleShould(JsonLdSerializerFixture fixture) : base(fixture) {}
    }
}