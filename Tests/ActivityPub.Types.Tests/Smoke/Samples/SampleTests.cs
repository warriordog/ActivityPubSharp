// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;
using ActivityPub.Types.Conversion;
using ActivityPub.Types.Tests.Util.Fixtures;

namespace ActivityPub.Types.Tests.Smoke.Samples;

public abstract class SampleTests : IClassFixture<JsonLdSerializerFixture>
{
    private readonly IJsonLdSerializer _jsonLdSerializer;
    protected SampleTests(JsonLdSerializerFixture fixture) => _jsonLdSerializer = fixture.JsonLdSerializer;

    protected void TestSample(Type expectedType, string sampleType) => TestSample(expectedType, sampleType, sampleType);

    protected void TestSample(Type expectedType, string sampleType, string sampleName)
    {
        // Quick check - not an assertion because this isn't part of the test.
        if (!expectedType.IsAssignableTo(typeof(ASType)))
            throw new ArgumentException("expected type must derive from ASType", nameof(expectedType));

        // Load sample
        var testInput = LoadJson(sampleName);

        // Test deserialize
        var valueObject = _jsonLdSerializer.Deserialize<ASType>(testInput);
        valueObject.Should().NotBeNull();
        valueObject.Should().BeOfType(expectedType);
        valueObject?.TypeMap.ASTypes.Should().Contain(sampleType);

        // Test serialize
        var valueJson = _jsonLdSerializer.SerializeToElement(valueObject);
        valueJson.Should().BeJsonObject();
        valueJson.Should().HaveASType(sampleType);
    }

    // I've done this many times in my career but I can NEVER remember how it works /annoyed
    // https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getmanifestresourcestream?view=net-7.0
    // https://stackoverflow.com/a/3314213
    private string LoadJson(string sampleName)
    {
        var jsonPath = $"{sampleName}.jsonld";
        var thisType = GetType();

        using var stream = thisType.Assembly.GetManifestResourceStream(thisType, jsonPath);
        if (stream == null)
            throw new ArgumentException($"Failed to load a JSON sample with name {jsonPath}");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}