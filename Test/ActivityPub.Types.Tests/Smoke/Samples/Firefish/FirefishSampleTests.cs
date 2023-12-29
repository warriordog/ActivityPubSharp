// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.AS.Collection;
using ActivityPub.Types.AS.Extended.Activity;
using ActivityPub.Types.AS.Extended.Actor;
using ActivityPub.Types.AS.Extended.Link;
using ActivityPub.Types.AS.Extended.Object;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Smoke.Samples.Firefish;

public class FirefishSampleTests(JsonLdSerializerFixture fixture)
    : SampleTests(fixture)
{

    [Fact]
    public void CreateShouldConvert() => TestSample<CreateActivity>(CreateActivity.CreateType);

    [Fact]
    public void HashtagShouldConvert() => TestSample<ASObject>("Hashtag");

    [Fact]
    public void ImageShouldConvert() => TestSample<ImageObject>(ImageObject.ImageType);

    [Fact]
    public void KeyShouldConvert() => TestSample<ASObject>("Key");

    [Fact]
    public void MentionShouldConvert() => TestSample<MentionLink>(MentionLink.MentionType);

    [Fact]
    public void NoteShouldConvert() => TestSample<NoteObject>(NoteObject.NoteType);

    [Fact]
    public void ObjectShouldConvert() => TestSample<ASObject>(ASObject.ObjectType);

    [Fact]
    public void OrderedCollectionShouldConvert() => TestSample<ASOrderedCollection>(ASOrderedCollection.OrderedCollectionType);

    [Fact]
    public void OrderedCollectionPageShouldConvert() => TestSample<ASOrderedCollectionPage>(ASOrderedCollectionPage.OrderedCollectionPageType);

    [Fact]
    public void PersonActorShouldConvert() => TestSample<PersonActor>(PersonActor.PersonType);

    [Fact]
    public void PropertyValueShouldConvert() => TestSample<ASObject>("PropertyValue");
}