// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Tests.Util.Fakes;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests;

public abstract class ASEntityTests
{
    public class DefiningContextShould : ASEntityTests
    {
        [Fact]
        public void DefaultToActivityStreams()
        {
            var entity = new ASTypeEntity();
            entity.DefiningContext.Should().BeSameAs(IJsonLDContext.ActivityStreams);
        }
        
        [Fact]
        public void CopyValueFromModel()
        {
            var entity = new EmptyExtendedTypeFakeEntity();
            entity.DefiningContext.Should().BeSameAs(EmptyExtendedTypeFake.DefiningContext);
        }

        [Fact]
        public void ReturnSharedInstance()
        {
            var entity = new ASTypeEntity();
            
            var first = entity.DefiningContext;
            var second = entity.DefiningContext;

            first.Should().BeSameAs(second);
        }
    }
}