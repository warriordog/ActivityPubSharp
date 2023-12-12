// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Conversion.Converters;
using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Tests.Util.Fakes;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Internal;

public abstract class AnonymousEntityPivotTests
{
    private TypeMapConverter.AnonymousEntityPivot PivotUnderTest { get; } = new();

    private DeserializationMetadata StubDeserializationMetadata { get; } = new()
    {
        TypeMap = new TypeMap(),
        JsonSerializerOptions = JsonSerializerOptions.Default,
        LDContext = new JsonLDContext()
    };
    
    public class ShouldConvertShould : AnonymousEntityPivotTests
    {
        [Fact]
        public void Throw_WhenTypeDoesNotImplementInterface()
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                PivotUnderTest.ShouldConvert(typeof(ASType), new JsonElement(), StubDeserializationMetadata);
            });
        }
        
        [Fact]
        public void ReturnFalse_WhenMethodReturnsFalse()
        {
            var result = PivotUnderTest.ShouldConvert(typeof(AnonymousExtensionFakeEntity), new JsonElement(), StubDeserializationMetadata);
            result.Should().BeFalse();
        }
        
        [Fact]
        public void ReturnTrue_WhenMethodReturnsTrue()
        {
            const string Json = """{"ExtendedString":"value"}""";
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(Json);
            var result = PivotUnderTest.ShouldConvert(typeof(AnonymousExtensionFakeEntity), jsonElement, StubDeserializationMetadata);
            result.Should().BeTrue();
        }
    }
}