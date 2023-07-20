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

    public void HaveProperty(string name) => Subject.TryGetProperty(name, out _).Should().BeTrue();
    public void NotHaveProperty(string name) => Subject.TryGetProperty(name, out _).Should().BeFalse();

    public void BeJsonString(string value)
    {
        Subject.ValueKind.Should().Be(JsonValueKind.String);
        Subject.GetString().Should().Be(value);
    }
}

public static class JsonElementAssertionsExtension
{
    public static JsonElementAssertions Should(this JsonElement element) => new(element);
}