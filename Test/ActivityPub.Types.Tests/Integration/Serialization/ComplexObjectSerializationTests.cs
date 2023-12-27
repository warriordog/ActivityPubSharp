// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.AS.Collection;
using ActivityPub.Types.AS.Extended.Activity;
using ActivityPub.Types.AS.Extended.Object;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public class ComplexObjectSerializationTests(JsonLdSerializerFixture fixture)
    : SerializationTests(fixture)
{

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
            Object = [follower],
            Target = [target],
        };
        ObjectUnderTest = new AcceptActivity
        {
            Actor = [target],
            Object = [follow],
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
            Content = "This is a note",
        };
        var collection = new ASCollection
        {
            Id = "https://home.example/actor/collections/1",
            Items =
            [
                "https://home.example/item/whatever",
                "https://home.example/item/whatever-2"
            ],
            TotalItems = 2
        };
        ObjectUnderTest = new AddActivity
        {
            Actor = [actor],
            Object = [note],
            Target = [collection]
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