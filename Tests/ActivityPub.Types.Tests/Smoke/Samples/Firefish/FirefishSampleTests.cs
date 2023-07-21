// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Collection;
using ActivityPub.Types.Extended.Actor;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Smoke.Samples.Firefish;

public class FirefishSampleTests : SampleTests
{
    public FirefishSampleTests(JsonLdSerializerFixture fixture) : base(fixture) {}

    [Fact]
    public void PersonActorShouldConvert() => TestSample(typeof(PersonActor), PersonActor.PersonType);

    [Fact]
    public void ASOrderedCollectionShouldConvert() => TestSample(typeof(ASOrderedCollection<ASType>), CollectionTypes.OrderedCollectionType);

    [Fact]
    public void ASOrderedCollectionPageShouldConvert() => TestSample(typeof(ASOrderedCollectionPage<ASType>), CollectionTypes.OrderedCollectionPageType);
}