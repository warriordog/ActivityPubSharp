// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using FluentAssertions.Primitives;

namespace ActivityPub.Types.Tests.Util.Assertions;

/// <summary>
/// Special assertions for <see cref="JsonElement"/>
/// </summary>
public class JsonElementAssertions : ObjectAssertions<JsonElement, JsonElementAssertions>
{
    public JsonElementAssertions(JsonElement value) : base(value) {}

    public void HaveProperty(string name)
    {
        BeJsonObject();
        Subject.TryGetProperty(name, out _).Should().BeTrue($"expected object to contain {name}, but no such property was found");
    }
    public void HaveProperty(string name, Predicate<JsonElement> predicate)
    {
        HaveProperty(name);
        var prop = Subject.GetProperty(name);
        var result = predicate(prop);
        result.Should().BeTrue("property value did not match the predicate");
    }
    public void NotHaveProperty(string name) => Subject.TryGetProperty(name, out _).Should().BeFalse($"expected object to not contain {name}, but it exists");

    public void HaveStringProperty(string name, string value)
    {
        HaveProperty(name);
        Subject.GetProperty(name).GetString().Should().Be(value);
    }

    public void BeJsonString() => Subject.ValueKind.Should().Be(JsonValueKind.String);
    public void BeJsonString(string value)
    {
        BeJsonString();
        Subject.GetString().Should().Be(value);
    }
    public void BeJsonObject() => Subject.ValueKind.Should().Be(JsonValueKind.Object);

    /// <summary>
    /// Asserts that the subject represents an object containing the provided AS type.
    /// String and array forms are supported.
    /// </summary>
    /// <param name="asType"></param>
    public void HaveASType(string asType)
    {
        BeJsonObject();
        HaveProperty("type");
        var type = Subject.GetProperty("type");
        switch (type.ValueKind)
        {
            case JsonValueKind.String:
                type.GetString().Should().Be(asType);
                break;
            case JsonValueKind.Array:
                type.EnumerateArray().ToList().Should().Contain(entry => entry.ValueKind == JsonValueKind.String && entry.GetString() == asType);
                break;
            case JsonValueKind.Null:
                Assert.Fail($"expected type to be \"{asType}\", but it was null");
                break;
            default:
                Assert.Fail($"expected type to be a string, array, or null, but it was {type.ValueKind}");
                break;
        }
    }
}

public static class JsonElementAssertionsExtension
{
    public static JsonElementAssertions Should(this JsonElement element) => new(element);
}