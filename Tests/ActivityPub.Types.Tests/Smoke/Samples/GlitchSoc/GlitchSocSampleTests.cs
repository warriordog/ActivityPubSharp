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
    public void AnnounceShouldConvert() => TestSample(typeof(AnnounceActivity), AnnounceActivity.AnnounceType);
    
    
    [Fact]
    public void CollectionShouldConvert() => TestSample(typeof(ASCollection<ASObject>), CollectionTypes.CollectionType);

    [Fact]
    public void CollectionPageShouldConvert() => TestSample(typeof(ASCollectionPage<ASObject>), CollectionTypes.CollectionPageType);

    [Fact]
    public void CreateShouldConvert() => TestSample(typeof(CreateActivity), CreateActivity.CreateType);

    [Fact]
    public void DocumentShouldConvert() => TestSample(typeof(DocumentObject), DocumentObject.DocumentType);
    
    [Fact]
    public void EmojiShouldConvert() => TestSample(typeof(ASObject), "Emoji");

    [Fact]
    public void HashtagShouldConvert() => TestSample(typeof(ASObject), "Hashtag");

    [Fact]
    public void ImageShouldConvert() => TestSample(typeof(ImageObject), ImageObject.ImageType);

    [Fact]
    public void KeyShouldConvert() => TestSample(typeof(ASObject), ASObject.ObjectType, "Key");

    [Fact]
    public void MentionShouldConvert() => TestSample(typeof(MentionLink), MentionLink.MentionType);

    [Fact]
    public void NoteShouldConvert() => TestSample(typeof(NoteObject), NoteObject.NoteType);

    [Fact]
    public void OrderedCollectionShouldConvert() => TestSample(typeof(ASOrderedCollection<ASObject>), CollectionTypes.OrderedCollectionType);

    [Fact]
    public void OrderedCollectionPageShouldConvert() => TestSample(typeof(ASOrderedCollectionPage<ASObject>), CollectionTypes.OrderedCollectionPageType);
    
    [Fact]
    public void PersonActorShouldConvert() => TestSample(typeof(PersonActor), PersonActor.PersonType);

    [Fact]
    public void PropertyValueShouldConvert() => TestSample(typeof(ASObject), "PropertyValue");
}