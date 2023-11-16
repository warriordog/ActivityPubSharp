// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Internal;

public abstract class SubTypePivotTests
{
    public class TryNarrowTypeShould : SubTypePivotTests
    {
        private SubTypePivot PivotUnderTest { get; } = new();

        private DeserializationMetadata Metadata { get; } = new()
        {
            LDContext = IJsonLDContext.ActivityStreams,
            JsonSerializerOptions = JsonSerializerOptions.Default,
            TypeMap = new TypeMap()
        };
        
        [Fact]
        public void ReturnFalseAndNull_WhenEntityDoesNotImplementInterface()
        {
            var result = PivotUnderTest.TryNarrowType(typeof(ASTypeEntity), new JsonElement(), Metadata, out var narrowType);

            result.Should().BeFalse();
            narrowType.Should().BeNull();
        }
        
        [Fact]
        public void ReturnFalseAndNull_WhenEntityReturnsFalse()
        {
            var result = PivotUnderTest.TryNarrowType(typeof(ASTransitiveActivityEntity), new JsonElement(), Metadata, out var narrowType);

            result.Should().BeFalse();
            narrowType.Should().BeNull();
        }

        [Fact]
        public void ReturnTrueAndType_WhenEntityReturnsTrue()
        {
            const string Json = """{"target":null}""";
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(Json);
            var result = PivotUnderTest.TryNarrowType(typeof(ASTransitiveActivityEntity), jsonElement, Metadata, out var narrowType);

            result.Should().BeTrue();
            narrowType.Should().Be(typeof(ASTargetedActivityEntity));
        }
    }
}