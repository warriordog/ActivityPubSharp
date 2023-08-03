// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Extended.Actor;
using ActivityPub.Types.Extended.Object;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Integration.Deserialization;

public abstract class SimpleObjectDeserializationTests : DeserializationTests<ASObject>
{
    private SimpleObjectDeserializationTests(JsonLdSerializerFixture fixture) : base(fixture)
    {
        JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Object"}""";
    }

    public class EmptyObject : SimpleObjectDeserializationTests
    {
        [Fact]
        public void ShouldIncludeContext()
        {
            ObjectUnderTest.JsonLdContext.Should().Contain("https://www.w3.org/ns/activitystreams");
        }

        [Fact]
        public void ShouldIncludeType()
        {
            ObjectUnderTest.Types.Should().Contain("Object");
        }

        public EmptyObject(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    public class Subclass : SimpleObjectDeserializationTests
    {
        [Fact]
        public void ShouldDeserializeToCorrectType()
        {
            JsonUnderTest = """{"@context":"https://www.w3.org/ns/activitystreams","type":"Image"}""";
            ObjectUnderTest.Should().BeOfType<ImageObject>();
        }

        [Fact]
        public void ShouldIncludePropertiesFromBaseTypes()
        {
            JsonUnderTest = """
            {
                "@context": "https://www.w3.org/ns/activitystreams",
                "type": "Person",
                "inbox": "https://example.com/actor/inbox",
                "outbox": "https://example.com/actor/outbox",
                "image": {
                    "type": "Image"
                },
                "id": "https://example.com/actor/id"
            }
            """;

            ObjectUnderTest.Should().BeOfType<PersonActor>();
            var personUnderTest = (PersonActor)ObjectUnderTest;
            personUnderTest.Inbox.HRef.Should().Be("https://example.com/actor/inbox");
            personUnderTest.Outbox.HRef.Should().Be("https://example.com/actor/outbox");
            personUnderTest.Image.Should().NotBeNull();
            personUnderTest.Id.Should().Be("https://example.com/actor/id");
        }

        public Subclass(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    public class ObjectWithUrl : SimpleObjectDeserializationTests
    {
        [Fact]
        public void ShouldDeserializeUrlList()
        {
            JsonUnderTest = """{"url":["https://example.com/example1", "https://example.com/example2"]}""";
            ObjectUnderTest.Url.Should().NotBeNull();
            ObjectUnderTest.Url.Should().HaveCount(2);
        }

        [Fact]
        public void ShouldDeserializeSingleUrl()
        {
            JsonUnderTest = """{"url":"https://example.com"}""";
            ObjectUnderTest.Url.Should().NotBeNull();
            ObjectUnderTest.Url.Should().HaveCount(1);
        }

        public ObjectWithUrl(JsonLdSerializerFixture fixture) : base(fixture) {}
    }

    public class FullObject : SimpleObjectDeserializationTests
    {
        [Fact]
        public void ShouldIncludeAllProperties()
        {
            JsonUnderTest =
                """
                {
                    "attachment":[{}],
                    "audience":[{}],
                    "bcc":[{}],
                    "bto":[{}],
                    "cc":[{}],
                    "context":{},
                    "generator":{},
                    "icon":{"type":"Image"},
                    "image":{"type":"Image"},
                    "inReplyTo":{},
                    "location":{},
                    "replies":{
                        "type":"Collection",
                        "totalItems":1,
                        "items":[{}]
                    },
                    "tag":[{}],
                    "to":[{}],
                    "url":"https://example.com",
                    "content":"content",
                    "duration":"PT5S",
                    "startTime":"2023-06-26T21:30:09.2872331-04:00",
                    "endTime":"2023-06-26T21:30:09.2873668-04:00",
                    "published":"2023-06-26T21:30:09.2874915-04:00",
                    "summary":"summary",
                    "updated":"2023-06-26T21:30:09.2877318-04:00",
                    "source":{},
                    "likes":"https://example.com/likes.collection",
                    "shares":"https://example.com/shares.collection",
                    "type":"Object",
                    "@context":"https://www.w3.org/ns/activitystreams",
                    "id":"https://example.com/some.uri",
                    "attributedTo":[{}],
                    "preview":{},
                    "name":"name",
                    "mediaType":"text/html"
                }
            """;

            ObjectUnderTest.Should().BeOfType<ASObject>();
            ObjectUnderTest.Attachment.Should().HaveCount(1);
            ObjectUnderTest.Audience.Should().HaveCount(1);
            ObjectUnderTest.BCC.Should().HaveCount(1);
            ObjectUnderTest.BTo.Should().HaveCount(1);
            ObjectUnderTest.CC.Should().HaveCount(1);
            ObjectUnderTest.Context.Should().NotBeNull();
            ObjectUnderTest.Generator.Should().NotBeNull();
            ObjectUnderTest.Icon.Should().NotBeNull();
            ObjectUnderTest.Image.Should().NotBeNull();
            ObjectUnderTest.InReplyTo.Should().NotBeNull();
            ObjectUnderTest.Location.Should().NotBeNull();

            ObjectUnderTest.Tag.Should().HaveCount(1);
            ObjectUnderTest.To.Should().HaveCount(1);
            ObjectUnderTest.Url.Should().NotBeNull();
            ObjectUnderTest.Url.Should().HaveCount(1);
            ObjectUnderTest.Content.Should().NotBeNull();
            ObjectUnderTest.Duration.Should().NotBeNull();
            ObjectUnderTest.StartTime.Should().NotBeNull();
            ObjectUnderTest.EndTime.Should().NotBeNull();
            ObjectUnderTest.Published.Should().NotBeNull();
            ObjectUnderTest.Summary.Should().NotBeNull();
            ObjectUnderTest.Updated.Should().NotBeNull();
            ObjectUnderTest.Source.Should().NotBeNull();
            ObjectUnderTest.Id.Should().NotBeNull();
            ObjectUnderTest.AttributedTo.Should().HaveCount(1);
            ObjectUnderTest.Preview.Should().NotBeNull();
            ObjectUnderTest.Name.Should().NotBeNull();
            ObjectUnderTest.MediaType.Should().NotBeNull();

            ObjectUnderTest.Replies.Should().NotBeNull();
            ObjectUnderTest.Replies!.HasItems.Should().BeTrue();
            ObjectUnderTest.Replies!.TotalItems.Should().Be(1);
            ObjectUnderTest.Replies!.Items!.Count.Should().Be(1);

            ObjectUnderTest.Likes?.HasLink.Should().BeTrue();
            ObjectUnderTest.Shares?.HasLink.Should().BeTrue();
        }

        public FullObject(JsonLdSerializerFixture fixture) : base(fixture) {}
    }
}