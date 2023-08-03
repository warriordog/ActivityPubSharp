// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Collection;
using ActivityPub.Types.Extended.Activity;
using ActivityPub.Types.Extended.Actor;
using ActivityPub.Types.Extended.Link;
using ActivityPub.Types.Extended.Object;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Smoke.Samples.GlitchSoc;

public class GlitchSocSampleTests : SampleTests
{
    public GlitchSocSampleTests(JsonLdSerializerFixture fixture) : base(fixture) {}

    [Fact]
    public void AnnounceShouldConvert() => TestSample(typeof(AnnounceActivity), AnnounceActivityEntity.AnnounceType);


    [Fact]
    public void CollectionShouldConvert() => TestSample(typeof(ASCollection), CollectionTypes.CollectionType);

    [Fact]
    public void CollectionPageShouldConvert() => TestSample(typeof(ASCollectionPage), CollectionTypes.CollectionPageType);

    [Fact]
    public void CreateShouldConvert() => TestSample(typeof(CreateActivity), CreateActivityEntity.CreateType);

    [Fact]
    public void DocumentShouldConvert() => TestSample(typeof(DocumentObject), DocumentObjectEntity.DocumentType);

    [Fact]
    public void EmojiShouldConvert() => TestSample(typeof(ASObject), "Emoji");

    [Fact]
    public void HashtagShouldConvert() => TestSample(typeof(ASObject), "Hashtag");

    [Fact]
    public void ImageShouldConvert() => TestSample(typeof(ImageObject), ImageObjectEntity.ImageType);

    [Fact]
    public void KeyShouldConvert() => TestSample(typeof(ASObject), ASObjectEntity.ObjectType, "Key");

    [Fact]
    public void MentionShouldConvert() => TestSample(typeof(MentionLink), MentionLinkEntity.MentionType);

    [Fact]
    public void NoteShouldConvert() => TestSample(typeof(NoteObject), NoteObjectEntity.NoteType);

    [Fact]
    public void OrderedCollectionShouldConvert() => TestSample(typeof(ASCollection), CollectionTypes.OrderedCollectionType);

    [Fact]
    public void OrderedCollectionPageShouldConvert() => TestSample(typeof(ASCollectionPage), CollectionTypes.OrderedCollectionPageType);

    [Fact]
    public void PersonActorShouldConvert() => TestSample(typeof(PersonActor), PersonActorEntity.PersonType);

    [Fact]
    public void PropertyValueShouldConvert() => TestSample(typeof(ASObject), "PropertyValue");
}