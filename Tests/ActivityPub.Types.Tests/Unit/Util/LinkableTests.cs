using ActivityPub.Types.Util;

// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Tests.Unit.Util;

public abstract class LinkableTests
{
    
    private ASLink Link { get; set; } = new() { HRef = "https://example.com/some.uri" };
    private Linkable<ASObject> WithLink { get; set; }
        
    private ASObject Value { get; set; } = new();
    private Linkable<ASObject> WithValue { get; set; }

    protected LinkableTests()
    {
        WithLink = new(Link);
        WithValue = new(Value);
    }
    
    public class ConstructorShould : LinkableTests
    {
        [Fact]
        public void SetProperties_FromASLink()
        {
            WithLink.HasValue.Should().BeFalse();
            WithLink.Value.Should().BeNull();
            WithLink.HasLink.Should().BeTrue();
            WithLink.Link.Should().Be(Link);
        }
        
        [Fact]
        public void SetProperties_FromValue()
        {
            WithValue.HasValue.Should().BeTrue();
            WithValue.Value.Should().Be(Value);
            WithValue.HasLink.Should().BeFalse();
            WithValue.Link.Should().BeNull();
        }
        
        [Fact]
        public void SetProperties_FromLinkable()
        {
            var second = new Linkable<ASObject>(WithValue);
            
            second.HasValue.Should().BeTrue();
            second.Value.Should().Be(Value);
            second.HasLink.Should().BeFalse();
            second.Link.Should().BeNull();
        }
    }

    public class ValueShould : LinkableTests
    {
        [Fact]
        public void HaveValue_WhenLinkableHasValue() => WithValue.Value.Should().Be(Value);

        [Fact]
        public void BeNull_WhenLinkableHasLink() => WithLink.Value.Should().BeNull();
    }

    public class LinkShould : LinkableTests
    {
        [Fact]
        public void BeNull_WhenLinkableHasValue() => WithValue.Link.Should().BeNull();

        [Fact]
        public void HaveLink_WhenLinkableHasLink() => WithLink.Link.Should().Be(Link);
    }

    public class TryGetLinkShould : LinkableTests
    {
        [Fact]
        public void ReturnTrue_WhenItHasLink() => WithLink.TryGetLink(out _).Should().BeTrue();

        [Fact]
        public void ReturnLink_WhenItHasLink()
        {
            WithLink.TryGetLink(out var link);
            link.Should().Be(Link);
        }

        [Fact]
        public void ReturnFalse_WhenItHasValue() => WithValue.TryGetLink(out _).Should().BeFalse();

        [Fact]
        public void ReturnNull_WhenItHasValue()
        {
            WithValue.TryGetLink(out var link);
            link.Should().BeNull();
        }
    }

    public class TryGetValueShould : LinkableTests
    {

        [Fact]
        public void ReturnFalse_WhenItHasLink() => WithLink.TryGetValue(out _).Should().BeFalse();

        [Fact]
        public void ReturnNull_WhenItHasLink()
        {
            WithLink.TryGetValue(out var link);
            link.Should().BeNull();
        }
        
        [Fact]
        public void ReturnTrue_WhenItHasValue() => WithValue.TryGetValue(out _).Should().BeTrue();

        [Fact]
        public void ReturnValue_WhenItHasValue()
        {
            WithValue.TryGetValue(out var value);
            value.Should().Be(value); 
        }
    }

    public class EqualsShould : LinkableTests
    {
        [Fact]
        public void ReturnFalse_WhenFirstHasLinkAndOtherHasValue() => WithLink.Equals(WithValue).Should().BeFalse();

        [Fact]
        public void ReturnFalse_WhenFirstHasValueAndOtherHasLink() => WithValue.Equals(WithLink).Should().BeFalse();

        [Fact]
        public void ReturnTrue_WhenBothHaveSameLink()
        {
            var other = new Linkable<ASObject>(Link);
            other.Equals(WithLink).Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_WhenLinksAreDifferent()
        {
            var other = new Linkable<ASObject>((ASLink)"https://example.com/some/other.uri");
            other.Equals(WithLink).Should().BeFalse();
        }

        [Fact]
        public void ReturnTrue_WhenBothHaveSameValue()
        {
            var other = new Linkable<ASObject>(Value);
            other.Equals(WithValue).Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_WhenValuesAreDifferent()
        {
            var other = new Linkable<ASObject>(new ASObject());
            other.Equals(WithValue).Should().BeFalse();
        }
    }

    public class OperatorTests : LinkableTests
    {
        [Fact]
        public void CastFromASLinkShould_SetLink()
        {
            Linkable<ASObject> linkable = Link;
            linkable.Link.Should().Be(Link);
        }

        [Fact]
        public void CastFromASUriShould_SetLink()
        {
            Linkable<ASObject> linkable = Link.HRef;
            linkable.Link.Should().BeEquivalentTo(Link);
        }

        [Fact]
        public void CastFromUriShould_SetLink()
        {
            Linkable<ASObject> linkable = Link.HRef.Uri;
            linkable.Link.Should().BeEquivalentTo(Link);
        }

        [Fact]
        public void CastFromStringShould_SetLink()
        {
            Linkable<ASObject> linkable = Link.HRef.Uri.ToString();
            linkable.Link.Should().BeEquivalentTo(Link);
        }

        [Fact]
        public void CastFromValueShould_SetValue()
        {
            Linkable<ASObject> linkable = Value;
            linkable.Value.Should().Be(Value);
        }

        [Fact]
        public void CastToASLinkShould_ReturnLink_WhenItHasLink()
        {
            ASLink? link = WithLink;
            link.Should().Be(Link);
        }

        [Fact]
        public void CastToASLinkShould_ReturnNull_WhenItHasValue()
        {
            ASLink? link = WithValue;
            link.Should().BeNull();
        }

        [Fact]
        public void CastToValueShould_ReturnValue_WhenItHasValue()
        {
            ASObject? value = WithValue;
            value.Should().Be(Value);
        }

        [Fact]
        public void CastToValueShould_ReturnNull_WhenItHasLink()
        {
            ASObject? value = WithLink;
            value.Should().BeNull();
        }
    }
}