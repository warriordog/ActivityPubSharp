// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Tests.Unit;

public abstract class ASLinkTests
{
    private const string UrlUnderTest = "https://example.com";

    public class HRefShould : ASLinkTests
    {
        [Fact]
        public void ImplicitlyCastToString()
        {
            string url = new ASLink
            {
                HRef = UrlUnderTest
            };
            url.Should().Be(UrlUnderTest);
        }

        [Fact]
        public void ImplicitlyCastFromString()
        {
            ASLink link = UrlUnderTest;
            link.HRef.Should().Be(UrlUnderTest);
        }
    }
}