// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Util;

public abstract class NaturalLanguageStringTests
{
    private NaturalLanguageString StringUnderTest { get; set; } = new();

    public class GetShould : NaturalLanguageStringTests
    {
        [Fact]
        public void ReturnNull_ForEmptyObject()
        {
            StringUnderTest.Get().Should().BeNull();
        }

        [Fact]
        public void ReturnNull_ForNoMatch()
        {
            StringUnderTest["en"] = "input";
            StringUnderTest.Get("es").Should().BeNull();
        }
        
        [Fact]
        public void ReturnDefaultValue_ForEmptyLanguage()
        {
            StringUnderTest.DefaultValue = "expected";
            StringUnderTest.Get().Should().Be("expected");
        }

        [Fact]
        public void ReturnExactValue_ForLanguage()
        {
            StringUnderTest["en", "us"] = "expected";
            StringUnderTest.Get("en", "us").Should().Be("expected");
        }

        [Fact]
        public void ReturnBaseValue_ForSubLanguage()
        {
            StringUnderTest["en"] = "expected";
            StringUnderTest.Get("en", "us").Should().Be("expected");
        }
    }

    public class GetExactlyShould : NaturalLanguageStringTests
    {
        [Fact]
        public void ReturnNull_ForEmptyObject()
        {
            StringUnderTest.GetExactly().Should().BeNull();
        }

        [Fact]
        public void ReturnNull_ForNoMatch()
        {
            StringUnderTest["en"] = "input";
            StringUnderTest.GetExactly("es").Should().BeNull();
        }
        
        [Fact]
        public void ReturnDefaultValue_ForEmptyLanguage()
        {
            StringUnderTest.DefaultValue = "expected";
            StringUnderTest.GetExactly().Should().Be("expected");
        }

        [Fact]
        public void ReturnExactValue_ForLanguage()
        {
            StringUnderTest["en", "us"] = "expected";
            StringUnderTest.GetExactly("en", "us").Should().Be("expected");
        }

        [Fact]
        public void ReturnNull_ForSubLanguage()
        {
            StringUnderTest["en"] = "expected";
            StringUnderTest.GetExactly("en", "us").Should().BeNull();
        }
    }

    public class HasShould : NaturalLanguageStringTests
    {
        [Fact]
        public void ReturnFalse_ForEmptyObject()
        {
            StringUnderTest.Has().Should().BeFalse();
        }

        [Fact]
        public void ReturnFalse_ForNoMatch()
        {
            StringUnderTest["en"] = "input";
            StringUnderTest.Has("es").Should().BeFalse();
        }
        
        [Fact]
        public void ReturnTrue_ForEmptyLanguage()
        {
            StringUnderTest.DefaultValue = "expected";
            StringUnderTest.Has().Should().BeTrue();
        }

        [Fact]
        public void ReturnTrue_ForLanguage()
        {
            StringUnderTest["en", "us"] = "expected";
            StringUnderTest.Has("en", "us").Should().BeTrue();
        }

        [Fact]
        public void ReturnTrue_ForSubLanguage()
        {
            StringUnderTest["en"] = "expected";
            StringUnderTest.Has("en", "us").Should().BeTrue();
        }
    }

    public class HasExactlyShould : NaturalLanguageStringTests
    {
        [Fact]
        public void ReturnFalse_ForEmptyObject()
        {
            StringUnderTest.HasExactly().Should().BeFalse();
        }

        [Fact]
        public void ReturnFalse_ForNoMatch()
        {
            StringUnderTest["en"] = "input";
            StringUnderTest.HasExactly("es").Should().BeFalse();
        }
        
        [Fact]
        public void ReturnTrue_ForEmptyLanguage()
        {
            StringUnderTest.DefaultValue = "expected";
            StringUnderTest.HasExactly().Should().BeTrue();
        }

        [Fact]
        public void ReturnTrue_ForLanguage()
        {
            StringUnderTest["en", "us"] = "expected";
            StringUnderTest.HasExactly("en", "us").Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_ForSubLanguage()
        {
            StringUnderTest["en"] = "expected";
            StringUnderTest.HasExactly("en", "us").Should().BeFalse();
        }
    }

    public class SetShould : NaturalLanguageStringTests
    {
        [Fact]
        public void SetDefaultValue_WhenNoLanguageIsProvided()
        {
            StringUnderTest.Set("expected");
            StringUnderTest.DefaultValue.Should().Be("expected");
        }

        [Fact]
        public void SetExactLanguage()
        {
            StringUnderTest.Set("expected", "en", "us");
            StringUnderTest.Get("en", "us").Should().Be("expected");
        }

        [Fact]
        public void SetBaseLanguage()
        {
            StringUnderTest.Set("expected", "en");
            StringUnderTest.Get("en", "us").Should().Be("expected");
        }

        [Fact]
        public void ReplaceExistingValue()
        {
            StringUnderTest.Set("wrong");
            StringUnderTest.Set("expected");
            StringUnderTest.Get("en", "us").Should().Be("expected");   
        }

        [Fact]
        public void ShadowDefaultValue()
        {
            StringUnderTest.Set("wrong");
            StringUnderTest.Set("expected", "en");
            StringUnderTest.Get("en", "us").Should().Be("expected");
        }

        [Fact]
        public void ShadowBaseLanguage()
        {
            StringUnderTest.Set("wrong", "en");
            StringUnderTest.Set("expected", "en", "us");
            StringUnderTest.Get("en", "us").Should().Be("expected");
        }
    }

    public class RemoveShould : NaturalLanguageStringTests
    {
        [Fact]
        public void RemoveLanguage()
        {
            StringUnderTest["en"] = "wrong";
            StringUnderTest.Remove("en");
            StringUnderTest.HasExactly("en").Should().BeFalse();
        }

        [Fact]
        public void LeaveSubLanguages()
        {
            StringUnderTest["en"] = "wrong";
            StringUnderTest["en", "us"] = "expected";
            StringUnderTest.Remove("en");
            StringUnderTest.HasExactly("en").Should().BeFalse();
            StringUnderTest.HasExactly("en", "us").Should().BeTrue();
        }

        [Fact]
        public void LeaveBaseLanguages()
        {
            StringUnderTest["en"] = "expected";
            StringUnderTest["en", "us"] = "wrong";
            StringUnderTest.Remove("en", "us");
            StringUnderTest.HasExactly("en").Should().BeTrue();
            StringUnderTest.HasExactly("en", "us").Should().BeFalse();
        }

        [Fact]
        public void RemoveDefaultValue_WhenNoLanguageIsProvided()
        {
            StringUnderTest.DefaultValue = "wrong";
            StringUnderTest.Remove();
            StringUnderTest.DefaultValue.Should().BeNull();
        }

        [Fact]
        public void DoNothing_WhenLanguageDoesntExist()
        {
            StringUnderTest.Remove("en");
            StringUnderTest.HasExactly("en").Should().BeFalse();
        }
    }

    public class FromLanguageMapShould : NaturalLanguageStringTests
    {
        [Fact]
        public void ConstructEmptyString_WhenMapIsEmpty()
        {
            StringUnderTest = NaturalLanguageString.FromLanguageMap(new Dictionary<string, string>());
            StringUnderTest.Has().Should().BeFalse();
        }

        [Fact]
        public void CopyAllLanguages()
        {
            var map = new Dictionary<string, string>
            {
                ["en"] = "english",
                ["es"] = "spanish"
            };

            StringUnderTest = NaturalLanguageString.FromLanguageMap(map);

            StringUnderTest["en"].Should().Be("english");
            StringUnderTest["es"].Should().Be("spanish");
        }
        
        [Fact]
        public void SplitSubTags()
        {
            var map = new Dictionary<string, string>
            {
                ["en"] = "english",
                ["en-us"] = "english US",
                ["en-gb"] = "english GB"
            };

            StringUnderTest = NaturalLanguageString.FromLanguageMap(map);
            
            StringUnderTest["en"].Should().Be("english");
            StringUnderTest["en", "us"].Should().Be("english US");
            StringUnderTest["en", "gb"].Should().Be("english GB");
        }
    }

    public class LanguageMapShould : NaturalLanguageStringTests
    {
        [Fact]
        public void NotIncludeDefaultValue()
        {
            StringUnderTest.DefaultValue = "wrong";
            StringUnderTest.LanguageMap.Should().BeEmpty();
        }

        [Fact]
        public void IncludeAllLanguages()
        {
            StringUnderTest["en"] = "english";
            StringUnderTest["es"] = "spanish";

            StringUnderTest.LanguageMap.Should()
                .HaveCount(2)
                .And.Contain("en", "english")
                .And.Contain("es", "spanish");
        }

        [Fact]
        public void MergeSubTags()
        {
            StringUnderTest["en"] = "english";
            StringUnderTest["en", "us"] = "english US";
            StringUnderTest["en", "gb"] = "english GB";

            StringUnderTest.LanguageMap.Should()
                .HaveCount(3)
                .And.Contain("en", "english")
                .And.Contain("en-us", "english US")
                .And.Contain("en-gb", "english GB");
        }

        [Fact]
        public void BeSharedInstance()
        {
            var first = StringUnderTest.LanguageMap;
            var second = StringUnderTest.LanguageMap;
            first.Should().BeSameAs(second);
        }

        [Fact]
        public void UpdateWhenStringChanges()
        {
            var map = StringUnderTest.LanguageMap;
            
            StringUnderTest["en"] = "wrong";
            StringUnderTest["en"] = "english";

            map.Should().Contain("en", "english");
        }
    }

    public class OperatorShould : NaturalLanguageStringTests
    {
        [Fact]
        public void ConstructWithDefaultValue()
        {
            StringUnderTest = "expected";
            StringUnderTest.DefaultValue.Should().Be("expected");
        }
    }
}