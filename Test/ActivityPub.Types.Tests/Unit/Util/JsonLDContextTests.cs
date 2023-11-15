// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Util;

public abstract class JsonLDContextTests
{
    public class ActivityStreamsShould : JsonLDContextTests
    {
        [Fact]
        public void IncludeASContext()
        {
            var context = IJsonLDContext.ActivityStreams;
            context.Should().Contain(JsonLDContextObject.ActivityStreams);
        }

        [Fact]
        public void IncludeNoOtherContexts()
        {
            var context = IJsonLDContext.ActivityStreams;
            context.Count.Should().Be(1);
        }
        
        [Fact]
        public void ReturnSharedInstance()
        {
            var first = IJsonLDContext.ActivityStreams;
            var second = IJsonLDContext.ActivityStreams;
            first.Should().BeSameAs(second);
        }
    }
    
    public class CreateASContextShould : JsonLDContextTests
    {
        [Fact]
        public void IncludeASContext()
        {
            var context = JsonLDContext.CreateASContext();
            context.Should().Contain(JsonLDContextObject.ActivityStreams);
        }

        [Fact]
        public void IncludeNoOtherContexts()
        {
            var context = JsonLDContext.CreateASContext();
            context.Count.Should().Be(1);
        }
        
        [Fact]
        public void ReturnNewInstance()
        {
            var first = JsonLDContext.CreateASContext();
            var second = JsonLDContext.CreateASContext();
            first.Should().NotBeSameAs(second);
        }
    }
}