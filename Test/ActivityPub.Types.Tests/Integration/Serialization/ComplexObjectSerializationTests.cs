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
    public ComplexObjectSerializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}

    [Fact]
    public void AcceptFollowWithObject_ShouldSerializeCorrectly()
    {
        var follower = new ASLink
        {
            HRef = "https://peer.example/actor",
        };
        var target = new ASLink
        {
            HRef = "https://home.example/actor",
        };
        var follow = new FollowActivity
        {
            Id = "https://peer.example/actor/activities/1",
            Object = new LinkableList<ASObject> { follower },
            Target = new LinkableList<ASObject> { target },
        };
        ObjectUnderTest = new AcceptActivity
        {
            Actor = new LinkableList<ASObject> { target },
            Object = new LinkableList<ASObject> { (ASObject)follow },
        };
        
        // Accept
        JsonUnderTest.Should()
            .BeJsonObject()
            .And.HaveStringProperty("type", "Accept")
            .And.HaveProperty("object");
        
        // Follow
        JsonUnderTest.GetProperty("object").Should()
            .BeJsonObject()
            .And.HaveStringProperty("type", "Follow")
            .And.HaveStringProperty("object", follower.HRef)
            .And.HaveStringProperty("target", target.HRef);
    }
    
    [Fact]
    public void AddNoteToCollection_ShouldSerializeCorrectly()
    {
        var actor = new ASLink
        {
            HRef = "https://home.example/actor",
        };
        var note = new NoteObject
        {
            Id = "https://peer.example/actor/activities/1",
            Content = new NaturalLanguageString("This is a note"),
        };
        var collection = new ASCollection
        {
            Id = "https://home.example/actor/collections/1",
            Items = new LinkableList<ASObject>(new ASLink[]
            {
                new(){HRef = "https://home.example/item/whatever"},
                new(){HRef = "https://home.example/item/whatever-2"}
            }),
            TotalItems = 2
        };
        ObjectUnderTest = new AddActivity
        {
            Actor = new LinkableList<ASObject> { actor },
            Object = new LinkableList<ASObject> { note },
            Target = new LinkableList<ASObject> { collection }
        };
        
        JsonUnderTest.Should()
            .BeJsonObject()
            .And.HaveStringProperty("type", "Add")
            .And.HaveProperty("object")
            .And.HaveProperty("target");
        JsonUnderTest.GetProperty("object").Should()
            .BeJsonObject()
            .And.HaveStringProperty("type", "Note");
        JsonUnderTest.GetProperty("target").Should()
            .BeJsonObject()
            .And.HaveStringProperty("type", "Collection")
            .And.HaveStringProperty("id", "https://home.example/actor/collections/1")
            .And.HaveArrayProperty("items");
    }
}