// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit;

public abstract class ASLinkTests
{
    private const string TestUriString = "https://example.com/some.uri";
    private Uri TestUri { get; set; } = new(TestUriString);
    private ASUri TestASUri { get; set; } = new(TestUriString);

    public class OperatorTests : ASLinkTests
    {
        [Fact]
        public void CastToStringShould_ReturnHRefAsString()
        {
            string url = new ASLink { HRef = TestASUri };
            url.Should().Be(TestUriString);
        }

        [Fact]
        public void CastFromStringShould_PopulateHRef()
        {
            ASLink link = TestUriString;
            link.HRef.Should().Be(TestASUri);
        }

        [Fact]
        public void CastToUriShould_ReturnHRefAsUri()
        {
            Uri uri = new ASLink { HRef = TestASUri };
            uri.Should().Be(TestUri);
        }

        [Fact]
        public void CastFromUriShould_PopulateHRef()
        {
            ASLink link = TestUri;
            link.HRef.Should().Be(TestASUri);
        }

        [Fact]
        public void CastToASLinkShould_ReturnHRef()
        {
            ASUri asUri = new ASLink { HRef = TestASUri };
            asUri.Should().Be(TestASUri);
        }

        [Fact]
        public void CastFromASLinkShould_PopulateHRef()
        {
            ASLink link = TestASUri;
            link.HRef.Should().Be(TestASUri);
        }
    }
 }