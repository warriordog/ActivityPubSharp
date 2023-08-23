// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.AS;

public abstract class ASObjectEntityTests
{
    public class TryNarrowTypeByJsonShould : ASObjectEntityTests
    {
        private readonly DeserializationMetadata _stubMetadata = new()
        {
            LDContext = new JsonLDContext(),
            JsonSerializerOptions = JsonSerializerOptions.Default,
            TypeMap = new TypeMap()
        };

        [Fact]
        public void ReturnAPActor_WhenJsonHasInboxAndOutbox()
        {
            var json = JsonSerializer.SerializeToElement(new { inbox = "i", outbox = "o" });
            var result = ASObjectEntity.TryNarrowTypeByJson(json, _stubMetadata, out var type);
            result.Should().BeTrue();
            type.Should().Be(typeof(APActorEntity));
        }

        [Fact]
        public void ReturnFalse_WhenJsonHasInboxOnly()
        {
            var json = JsonSerializer.SerializeToElement(new { inbox = "i" });
            var result = ASObjectEntity.TryNarrowTypeByJson(json, _stubMetadata, out var type);
            result.Should().BeFalse();
            type.Should().BeNull();
        }

        [Fact]
        public void ReturnFalse_WhenJsonHasOutboxOnly()
        {
            var json = JsonSerializer.SerializeToElement(new { outbox = "o" });
            var result = ASObjectEntity.TryNarrowTypeByJson(json, _stubMetadata, out var type);
            result.Should().BeFalse();
            type.Should().BeNull();
        }

        [Fact]
        public void ReturnFalse_WhenJsonHasNeither()
        {
            var json = JsonSerializer.SerializeToElement(new {});
            var result = ASObjectEntity.TryNarrowTypeByJson(json, _stubMetadata, out var type);
            result.Should().BeFalse();
            type.Should().BeNull();
        }
    }
}