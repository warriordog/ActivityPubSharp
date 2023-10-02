// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Util;

public class LinkRelTests
{
    private string InputString { get; set; } = "";
    private LinkRel LinkUnderTest => new(InputString);

    [Fact]
    public void ShouldWrapValidString()
    {
        InputString = "me";
        LinkUnderTest.Value.Should().Be(InputString);
    }

    [Fact]
    public void ShouldThrow_IfStringContainsSpace()
    {
        InputString = "has space";
        Assert.Throws<ArgumentException>(() => LinkUnderTest);
    }

    [Fact]
    public void ShouldThrow_IfStringContainsTab()
    {
        InputString = "has\ttab";
        Assert.Throws<ArgumentException>(() => LinkUnderTest);
    }

    [Fact]
    public void ShouldThrow_IfStringContainsLineFeed()
    {
        InputString = "has\nlf";
        Assert.Throws<ArgumentException>(() => LinkUnderTest);
    }

    [Fact]
    public void ShouldThrow_IfStringContainsCarriageReturn()
    {
        InputString = "has\rcr";
        Assert.Throws<ArgumentException>(() => LinkUnderTest);
    }

    [Fact]
    public void ShouldThrow_IfStringContainsComma()
    {
        InputString = "has,comma";
        Assert.Throws<ArgumentException>(() => LinkUnderTest);
    }
}