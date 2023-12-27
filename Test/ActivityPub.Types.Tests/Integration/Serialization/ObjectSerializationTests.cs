// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Tests.Util.Fixtures;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public abstract class ObjectSerializationTests : SerializationTests
{
    public class ContentPropertyShould : ObjectSerializationTests
    {
        [Fact]
        public void WriteAllLanguagesToContentMapJson()
        {
            ObjectUnderTest = new ASObject
            {
                Content = new NaturalLanguageString
                {
                    ["en"] = "english",
                    ["es"] = "spanish"
                }
            };

            JsonUnderTest.Should().HaveObjectProperty("contentMap");
            JsonUnderTest.GetProperty("contentMap").Should()
                .HaveStringProperty("en", "english")
                .And.HaveStringProperty("es", "spanish");
        }

        [Fact]
        public void FlattenSubLanguages()
        {
            ObjectUnderTest = new ASObject
            {
                Content = new NaturalLanguageString
                {
                    ["en", "us"] = "english US",
                    ["en", "gb"] = "english GB",
                    ["es"] = "spanish"
                }
            };

            JsonUnderTest.Should().HaveObjectProperty("contentMap");
            JsonUnderTest.GetProperty("contentMap").Should()
                .HaveStringProperty("en-us", "english US")
                .And.HaveStringProperty("en-gb", "english GB")
                .And.HaveStringProperty("es", "spanish");
        }

        [Fact]
        public void WriteDefaultValueToContentJson()
        {
            ObjectUnderTest = new ASObject
            {
                Content = new NaturalLanguageString
                {
                    DefaultValue = "default"
                }
            };

            JsonUnderTest.Should().HaveStringProperty("content", "default");
            JsonUnderTest.Should().NotHaveProperty("contentMap");
        }

        [Fact]
        public void WriteDefaultValueToContentJson_AndLanguagesToContentMapJson()
        {            
            ObjectUnderTest = new ASObject
            {
                Content = new NaturalLanguageString
                {
                    DefaultValue = "default",
                    ["en"] = "english",
                    ["en", "us"] = "english US",
                    ["es"] = "spanish"
                }
            };
            
            
            JsonUnderTest.Should()
                .HaveStringProperty("content", "default")
                .And.HaveObjectProperty("contentMap");
            JsonUnderTest.GetProperty("contentMap").Should()
                .HaveStringProperty("en", "english")
                .And.HaveStringProperty("en-us", "english US")
                .And.HaveStringProperty("es", "spanish");
        }

        [Fact]
        public void ExcludeContentMapJson_WhenEmpty()
        {
            ObjectUnderTest = new ASObject
            {
                Content = new NaturalLanguageString
                {
                    DefaultValue = "default"
                }
            };

            JsonUnderTest.Should().NotHaveProperty("contentMap");
        }
        
        public ContentPropertyShould(JsonLdSerializerFixture fixture) : base(fixture) {}
    }
    
    protected ObjectSerializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}
}