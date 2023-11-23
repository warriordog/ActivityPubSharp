// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using FluentAssertions.Primitives;

namespace ActivityPub.Types.Tests.Util.Assertions;

/// <summary>
///     Special assertions for <see cref="JsonElement" />
/// </summary>
public class JsonElementAssertions : ObjectAssertions<JsonElement, JsonElementAssertions>
{
    public JsonElementAssertions(JsonElement value) : base(value) {}

    public AndConstraint<JsonElementAssertions> HaveProperty(string name)
    {
        BeJsonObject();

        if (!Subject.TryGetProperty(name, out _))
            Assert.Fail($"Expected object to contain property {name}, but it does not");

        return new AndConstraint<JsonElementAssertions>(this);
    }

    public AndConstraint<JsonElementAssertions> HaveProperty(string name, Action<JsonElement> inspector)
    {
        HaveProperty(name);
        var prop = Subject.GetProperty(name);
        inspector(prop);

        return new AndConstraint<JsonElementAssertions>(this);
    }

    public AndConstraint<JsonElementAssertions> NotHaveProperty(string name)
    {
        BeJsonObject();

        if (Subject.TryGetProperty(name, out _))
            Assert.Fail($"Expected object to not contain property {name}, but it does");

        return new AndConstraint<JsonElementAssertions>(this);
    }

    public AndConstraint<JsonElementAssertions> HaveStringProperty(string name, string value)
    {
        HaveProperty(name);
        Subject.GetProperty(name).Should().BeJsonString(value);

        return new AndConstraint<JsonElementAssertions>(this);
    }

    public AndConstraint<JsonElementAssertions> HaveArrayProperty(string name)
    {
        HaveProperty(name);
        Subject.GetProperty(name).ValueKind.Should().Be(JsonValueKind.Array);

        return new AndConstraint<JsonElementAssertions>(this);
    }

    public AndConstraint<JsonElementAssertions> BeJsonString()
    {
        Subject.ValueKind.Should().Be(JsonValueKind.String);

        return new AndConstraint<JsonElementAssertions>(this);
    }

    public AndConstraint<JsonElementAssertions> BeJsonString(string value)
    {
        BeJsonString();
        Subject.GetString().Should().Be(value);

        return new AndConstraint<JsonElementAssertions>(this);
    }

    public AndConstraint<JsonElementAssertions> BeJsonObject()
    {
        Subject.ValueKind.Should().Be(JsonValueKind.Object);

        return new AndConstraint<JsonElementAssertions>(this);
    }

    public AndConstraint<JsonElementAssertions> BeJsonArray()
    {
        Subject.ValueKind.Should().Be(JsonValueKind.Array);

        return new AndConstraint<JsonElementAssertions>(this);
    }

    /// <summary>
    ///     Asserts that the subject represents an object containing the provided AS type.
    ///     String and array forms are supported.
    /// </summary>
    /// <param name="asType"></param>
    public AndConstraint<JsonElementAssertions> HaveASType(string asType)
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
                Assert.Fail($"Expected property \"type\" to be \"{asType}\", but it was null");
                break;
            default:
                Assert.Fail($"Expected property \"type\" to be a string, array, or null, but it was {type.ValueKind}");
                break;
        }

        return new AndConstraint<JsonElementAssertions>(this);
    }
}

public static class JsonElementAssertionsExtension
{
    public static JsonElementAssertions Should(this JsonElement element) => new(element);
}