// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Nodes;

namespace ActivityPub.Types.Json;

/// <summary>
/// A type that can convert itself to/from JSON.
/// This is a TEMPORARY workaround for limitations in System.Text.Json
/// </summary>
public interface IJsonConvertible<TThis>
    where TThis : IJsonConvertible<TThis>
{
    // TODO docs
    //
    // public void ReadJson(TThis obj, JsonElement element, JsonOptions options);
    // public void WriteJson(TThis obj, JsonNode node, JsonOptions options);
    //

    public static abstract TThis? Deserialize(JsonElement element, JsonOptions options);
    public static abstract JsonNode? Serialize(TThis obj, JsonOptions options);
}