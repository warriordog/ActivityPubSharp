// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Util;

public abstract class JsonLDContextObjectTests
{
    public class ActivityStreamsShould : JsonLDContextObjectTests
    {
        [Fact]
        public void ConstructLinkObject()
        {
            var context = JsonLDContextObject.ActivityStreams;
            context.IsExternal.Should().BeTrue();
        }

        [Fact]
        public void LinkToASContext()
        {
            var context = JsonLDContextObject.ActivityStreams;
            context.ExternalLink?.Should().Be("https://www.w3.org/ns/activitystreams");
        }

        [Fact]
        public void ReturnSharedInstance()
        {
            var first = JsonLDContextObject.ActivityStreams;
            var second = JsonLDContextObject.ActivityStreams;
            first.Should().BeSameAs(second);
        }
    }
    
    public class EqualsShould : JsonLDContextObjectTests
    {
        [Fact]
        public void BeTrue_WhenLinksMatch()
        {
            var first = new JsonLDContextObject("https://example.com");
            var second = new JsonLDContextObject("https://example.com");
            first.Equals(second).Should().BeTrue();
        }

        [Fact]
        public void BeFalse_WhenLinksDontMatch()
        {
            var first = new JsonLDContextObject("https://example.com/first");
            var second = new JsonLDContextObject("https://example.com/second");
            first.Equals(second).Should().BeFalse();
        }

        [Fact]
        public void BeFalse_WhenOneHasLinkAndOtherHasTerms()
        {
            var first = new JsonLDContextObject("https://example.com/first");
            var second = new JsonLDContextObject(new Dictionary<string, JsonLDTerm>());
            first.Equals(second).Should().BeFalse();
        }

        [Fact]
        public void BeTrue_WhenObjectsHaveNoTerms()
        {
            var first = new JsonLDContextObject(new Dictionary<string, JsonLDTerm>());
            var second = new JsonLDContextObject(new Dictionary<string, JsonLDTerm>());
            first.Equals(second).Should().BeTrue();
        }

        [Fact]
        public void BeTrue_WhenObjectsHaveSameSingleTerm()
        {
            var first = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["term"] = new() { Id = "id" }
                }
            );
            var second = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["term"] = new() { Id = "id" }
                }
            );
            first.Equals(second).Should().BeTrue();
        }

        [Fact]
        public void BeTrue_WhenObjectsContainSameTerms()
        {
            var first = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["first"] = new() { Id = "1" },
                    ["second"] = new() { Id = "2" }
                }
            );
            var second = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["first"] = new() { Id = "1" },
                    ["second"] = new() { Id = "2" }
                }
            );
            first.Equals(second).Should().BeTrue();
        }

        [Fact]
        public void BeFalse_WhenOneIsEmptyAndOneHasContents()
        {
            var first = new JsonLDContextObject(new Dictionary<string, JsonLDTerm>());
            var second = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["term"] = new() { Id = "id" }
                }
            );
            first.Equals(second).Should().BeFalse();
        }

        [Fact]
        public void BeFalse_WhenContentsAreDifferent()
        {
            var first = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["term"] = new() { Id = "first" }
                }
            );
            var second = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["term"] = new() { Id = "second" }
                }
            );
            first.Equals(second).Should().BeFalse();
        }
    }

    public class HashCodeShould : JsonLDContextObjectTests
    {
        [Fact]
        public void BeEqual_WhenLinksMatch()
        {
            var first = new JsonLDContextObject("https://example.com");
            var second = new JsonLDContextObject("https://example.com");
            first.GetHashCode().Should().Be(second.GetHashCode());
        }

        [Fact]
        public void BeDifferent_WhenLinksDontMatch()
        {
            var first = new JsonLDContextObject("https://example.com/first");
            var second = new JsonLDContextObject("https://example.com/second");
            first.GetHashCode().Should().NotBe(second.GetHashCode());
        }

        [Fact]
        public void BeDifferent_WhenOneHasLinkAndOtherHasTerms()
        {
            var first = new JsonLDContextObject("https://example.com/first");
            var second = new JsonLDContextObject(new Dictionary<string, JsonLDTerm>());
            first.GetHashCode().Should().NotBe(second.GetHashCode());
        }

        [Fact]
        public void BeEqual_WhenObjectsHaveNoTerms()
        {
            var first = new JsonLDContextObject(new Dictionary<string, JsonLDTerm>());
            var second = new JsonLDContextObject(new Dictionary<string, JsonLDTerm>());
            first.GetHashCode().Should().Be(second.GetHashCode());
        }

        [Fact]
        public void BeEqual_WhenObjectsHaveSameSingleTerm()
        {
            var first = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["term"] = new() { Id = "id" }
                }
            );
            var second = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["term"] = new() { Id = "id" }
                }
            );
            first.GetHashCode().Should().Be(second.GetHashCode());
        }

        [Fact]
        public void BeEqual_WhenObjectsContainSameTerms()
        {
            var first = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["first"] = new() { Id = "1" },
                    ["second"] = new() { Id = "2" }
                }
            );
            var second = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["first"] = new() { Id = "1" },
                    ["second"] = new() { Id = "2" }
                }
            );
            first.GetHashCode().Should().Be(second.GetHashCode());
        }

        [Fact]
        public void BeDifferent_WhenOneIsEmptyAndOneHasContents()
        {
            var first = new JsonLDContextObject(new Dictionary<string, JsonLDTerm>());
            var second = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["term"] = new() { Id = "id" }
                }
            );
            first.GetHashCode().Should().NotBe(second.GetHashCode());
        }

        [Fact]
        public void BeDifferent_WhenContentsAreDifferent()
        {
            var first = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["term"] = new() { Id = "first" }
                }
            );
            var second = new JsonLDContextObject(
                new Dictionary<string, JsonLDTerm>
                {
                    ["term"] = new() { Id = "second" }
                }
            );
            first.GetHashCode().Should().NotBe(second.GetHashCode());
        }
    }
}