// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.AS.Collection;
using ActivityPub.Types.AS.Extended.Actor;
using ActivityPub.Types.AS.Extended.Object;
using ActivityPub.Types.Tests.Util.Fixtures;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public abstract class SimpleObjectSerializationTests : SerializationTests
{
    private SimpleObjectSerializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}

    public class EmptyObject(JsonLdSerializerFixture fixture)
        : SimpleObjectSerializationTests(fixture)
    {

        [Fact]
        public void ShouldWriteObject()
        {
            ObjectUnderTest = new ASObject();
            JsonUnderTest.Should().BeJsonObject();
        }

        [Fact]
        public void ShouldIncludeContext()
        {
            ObjectUnderTest = new ASObject();
            JsonUnderTest.Should().HaveStringProperty("@context", "https://www.w3.org/ns/activitystreams");
        }

        [Fact]
        public void ShouldIncludeType()
        {
            ObjectUnderTest = new ASObject();
            JsonUnderTest.Should().HaveASType(ASObject.ObjectType);
        }

        [Fact]
        public void ShouldIncludeOnlyTypeAndContext()
        {
            ObjectUnderTest = new ASObject();

            var props = JsonUnderTest.EnumerateObject().ToList();

            props.Should().HaveCount(2);
            props.Should().Contain(p => p.Name == "type");
            props.Should().Contain(p => p.Name == "@context");
        }
    }

    public class Subclass(JsonLdSerializerFixture fixture)
        : SimpleObjectSerializationTests(fixture)
    {

        [Fact]
        public void ShouldSerializeToCorrectType()
        {
            ObjectUnderTest = new ImageObject();
            JsonUnderTest.Should().HaveASType(ImageObject.ImageType);
        }

        [Fact]
        public void ShouldIncludePropertiesFromBaseTypes()
        {
            ObjectUnderTest = new PersonActor
            {
                // From APActor
                Inbox = "https://example.com/actor/inbox",
                Outbox = "https://example.com/actor/outbox",
                SharedInbox = "https://example.com/actor/shared_inbox",

                // From ASObject
                Image = new ImageObject(),

                // From ASType
                Id = "https://example.com/actor/id"
            };

            JsonUnderTest.Should()
                .HaveStringProperty("inbox", "https://example.com/actor/inbox")
                .And.HaveStringProperty("outbox", "https://example.com/actor/outbox")
                .And.HaveStringProperty("id", "https://example.com/actor/id")
                .And.HaveObjectProperty("image");
            JsonUnderTest.GetProperty("image").Should()
                .HaveASType("Image");
        }
    }

    public class FullObject(JsonLdSerializerFixture fixture)
        : SimpleObjectSerializationTests(fixture)
    {

        [Fact]
        public void ShouldIncludeAllProperties()
        {
            ObjectUnderTest = new ASObject
            {
                // From ASObject
                Attachment = [new ASObject()],
                Audience = [new ASObject()],
                BCC = [new ASObject()],
                BTo = [new ASObject()],
                CC = [new ASObject(), new ASObject()],
                Context = "https://example.com/some/context", // this is the worst field name
                Generator = new ASObject(),
                Icon = new ImageObject(),
                Image = new ImageObject(),
                InReplyTo = new ASObject(),
                Location = new ASObject(),
                Replies = new ASCollection
                {
                    Items = new LinkableList<ASObject> { new ASObject() }
                },
                Tag = [new ASObject()],
                To = [new ASObject()],
                Url = ["https://example.com"],
                Content = "content",
                Duration = "PT5S",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Published = DateTime.Now,
                Summary = "summary",
                Updated = DateTime.Now,
                Source = new ASObject(),
                Likes = "https://example.com/likes.collection",
                Shares = "https://example.com/shares.collection",

                // From ASType
                Id = "https://example.com/some.uri",
                AttributedTo = [new ASObject()],
                Preview = new ASObject(),
                Name = "name",
                MediaType = "text/html"
            };

            // From ASObject
            JsonUnderTest.Should().HaveObjectProperty("attachment")
                .And.HaveObjectProperty("audience")
                .And.HaveObjectProperty("bcc")
                .And.HaveObjectProperty("bto")
                .And.HaveArrayProperty("cc")
                .And.HaveStringProperty("context", "https://example.com/some/context")
                .And.HaveObjectProperty("generator")
                .And.HaveObjectProperty("icon")
                .And.HaveObjectProperty("image")
                .And.HaveObjectProperty("inReplyTo")
                .And.HaveObjectProperty("location")
                .And.HaveObjectProperty("replies")
                .And.HaveObjectProperty("tag")
                .And.HaveObjectProperty("to")
                .And.HaveStringProperty("url", "https://example.com/")
                .And.HaveStringProperty("content", "content")
                .And.HaveStringProperty("duration", "PT5S")
                .And.HaveStringProperty("startTime")
                .And.HaveStringProperty("endTime")
                .And.HaveStringProperty("published")
                .And.HaveStringProperty("summary", "summary")
                .And.HaveStringProperty("updated")
                .And.HaveObjectProperty("source")
                .And.HaveStringProperty("likes", "https://example.com/likes.collection")
                .And.HaveStringProperty("shares", "https://example.com/shares.collection")

                // From ASType
                .And.HaveStringProperty("id", "https://example.com/some.uri")
                .And.HaveObjectProperty("attributedTo")
                .And.HaveObjectProperty("preview")
                .And.HaveStringProperty("name", "name")
                .And.HaveStringProperty("mediaType", "text/html");
        }

        public class UntypedObjectShould(JsonLdSerializerFixture fixture)
            : SimpleObjectSerializationTests(fixture)
        {
            [Fact]
            public void NotIncludeTypeProperty()
            {
                ObjectUnderTest = new ASType();
                JsonUnderTest.Should().NotHaveProperty("type");
            }

        }
    }
}