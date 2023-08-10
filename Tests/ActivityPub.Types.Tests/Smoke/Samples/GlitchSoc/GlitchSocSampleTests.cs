// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.AS.Collection;
using ActivityPub.Types.AS.Extended.Activity;
using ActivityPub.Types.AS.Extended.Actor;
using ActivityPub.Types.AS.Extended.Link;
using ActivityPub.Types.AS.Extended.Object;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Smoke.Samples.GlitchSoc;

public class GlitchSocSampleTests : SampleTests
{
    public GlitchSocSampleTests(JsonLdSerializerFixture fixture) : base(fixture) {}

    [Fact]
    public void AnnounceShouldConvert() => TestSample<AnnounceActivity>(AnnounceActivityEntity.AnnounceType);


    [Fact]
    public void CollectionShouldConvert() => TestSample<ASCollection>(ASCollectionEntity.CollectionType);

    [Fact]
    public void CollectionPageShouldConvert() => TestSample<ASCollectionPage>(ASCollectionPageEntity.CollectionPageType);

    [Fact]
    public void CreateShouldConvert() => TestSample<CreateActivity>(CreateActivityEntity.CreateType);

    [Fact]
    public void DocumentShouldConvert() => TestSample<DocumentObject>(DocumentObjectEntity.DocumentType);

    [Fact]
    public void EmojiShouldConvert() => TestSample<ASObject>("Emoji");

    [Fact]
    public void HashtagShouldConvert() => TestSample<ASObject>("Hashtag");

    [Fact]
    public void ImageShouldConvert() => TestSample<ImageObject>(ImageObjectEntity.ImageType);

    [Fact]
    public void KeyShouldConvert() => TestSample<ASObject>(ASObjectEntity.ObjectType, "Key");

    [Fact]
    public void MentionShouldConvert() => TestSample<MentionLink>(MentionLinkEntity.MentionType);

    [Fact]
    public void NoteShouldConvert() => TestSample<NoteObject>(NoteObjectEntity.NoteType);

    [Fact]
    public void OrderedCollectionShouldConvert() => TestSample<ASOrderedCollection>(ASOrderedCollectionEntity.OrderedCollectionType);

    [Fact]
    public void OrderedCollectionPageShouldConvert() => TestSample<ASOrderedCollectionPage>(ASOrderedCollectionPageEntity.OrderedCollectionPageType);

    [Fact]
    public void PersonActorShouldConvert() => TestSample<PersonActor>(PersonActorEntity.PersonType);

    [Fact]
    public void PropertyValueShouldConvert() => TestSample<ASObject>("PropertyValue");
}