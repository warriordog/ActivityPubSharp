// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.AS.Collection;
using ActivityPub.Types.AS.Extended.Activity;
using ActivityPub.Types.AS.Extended.Object;
using ActivityPub.Types.Tests.Util.Fixtures;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public class ComplexObjectSerializationTests : SerializationTests
{
    public ComplexObjectSerializationTests(JsonLdSerializerFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public void AcceptFollowWithObjectTest()
    {
        var follower = new ASLink()
        {
            HRef = "https://peer.example/actor",
        };
        var target = new ASLink()
        {
            HRef = "https://home.example/actor",
        };
        var follow = new FollowActivity()
        {
            Id = "https://peer.example/actor/activities/1",
            Object = new LinkableList<ASObject>() { follower },
            Target = new LinkableList<ASObject>() { target },
        };
        ObjectUnderTest = new AcceptActivity()
        {
            Actor = new LinkableList<ASObject>() { target },
            Object = new LinkableList<ASObject>() { (ASObject)follow },
        };
        // Accept
        JsonUnderTest.Should().HaveStringProperty("type", "Accept");
        JsonUnderTest.Should().HaveProperty("object");
        JsonUnderTest.GetProperty("object").Should().HaveStringProperty("type", "Follow");
        
        // Follow
        JsonUnderTest.GetProperty("object").Should().HaveProperty("object");
        JsonUnderTest.GetProperty("object").GetProperty("object").Should().BeJsonString(follower.HRef);
        JsonUnderTest.GetProperty("object").Should().HaveProperty("target");
        JsonUnderTest.GetProperty("object").GetProperty("target").Should().BeJsonString(target.HRef);
    }
    
    [Fact]
    public void AddNoteToCollectionTest()
    {
        var actor = new ASLink()
        {
            HRef = "https://home.example/actor",
        };
        var note = new NoteObject()
        {
            Id = "https://peer.example/actor/activities/1",
            Content = new NaturalLanguageString("This is a note"),
        };
        var collection = new ASCollection()
        {
            Id = "https://home.example/actor/collections/1"
        };
        ObjectUnderTest = new AddActivity()
        {
            Actor = new LinkableList<ASObject>() { actor },
            Object = new LinkableList<ASObject>() { (ASObject)note },
            Target = new LinkableList<ASObject>(collection)
        };
        
        JsonUnderTest.Should().HaveStringProperty("type", "Add");
        JsonUnderTest.Should().HaveProperty("object");
        JsonUnderTest.GetProperty("object").Should().HaveStringProperty("type", "Note");
        // Fails, I think because the collection is empty, so it gets excluded from serialization
        JsonUnderTest.GetProperty("target").Should().HaveStringProperty("type", "Collection"); 
    }
}