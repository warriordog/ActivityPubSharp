// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.AS.Extended.Actor;
using ActivityPub.Types.AS.Extended.Object;

namespace ActivityPub.Types.Tests.Unit.AS;

public abstract class APActorTests
{
    public class ExtendConstructorShould : APActorTests
    {
        [Fact]
        public void ExtendRegularObject()
        {
            var note = new NoteObject()
            {
                Id = "https://example.com/object",
                Content = "content"
            };

            var actor = new APActor(note)
            {
                Inbox = "https://example.com/inbox",
                Outbox = "https://example.com/outbox"
            };

            actor.Is<NoteObject>().Should().BeTrue();
            note.Is<APActor>().Should().BeTrue();
        }

        [Fact]
        public void NoOpWhenWrappingAnotherActor()
        {
            var person = new PersonActor()
            {
                Inbox = "https://example.com/inbox",
                Outbox = "https://example.com/outbox"
            };
            var actor = new APActor(person)
            {
                Inbox = person.Inbox,
                Outbox = person.Outbox
            };

            person.Inbox = "https://example.com/changed";

            actor.Inbox.Should().Be(person.Inbox);
        }
    }
}