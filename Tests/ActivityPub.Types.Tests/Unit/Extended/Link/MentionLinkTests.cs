// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS.Extended.Link;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Extended.Link;

public abstract class MentionLinkTests
{
    private const string TestUriString = "https://example.com/some.uri";
    private Uri TestUri { get; } = new(TestUriString);
    private ASUri TestASUri { get; } = new(TestUriString);

    public class OperatorTests : MentionLinkTests
    {
        [Fact]
        public void CastToStringShould_ReturnHRefAsString()
        {
            string url = new MentionLink { HRef = TestASUri };
            url.Should().Be(TestUriString);
        }

        [Fact]
        public void CastFromStringShould_PopulateHRef()
        {
            MentionLink link = TestUriString;
            link.HRef.Should().Be(TestASUri);
        }

        [Fact]
        public void CastToUriShould_ReturnHRefAsUri()
        {
            Uri uri = new MentionLink { HRef = TestASUri };
            uri.Should().Be(TestUri);
        }

        [Fact]
        public void CastFromUriShould_PopulateHRef()
        {
            MentionLink link = TestUri;
            link.HRef.Should().Be(TestASUri);
        }

        [Fact]
        public void CastToMentionLinkShould_ReturnHRef()
        {
            ASUri asUri = new MentionLink { HRef = TestASUri };
            asUri.Should().Be(TestASUri);
        }

        [Fact]
        public void CastFromMentionLinkShould_PopulateHRef()
        {
            MentionLink link = TestASUri;
            link.HRef.Should().Be(TestASUri);
        }
    }
}