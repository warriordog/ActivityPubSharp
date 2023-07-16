// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
/// Base type for all activities that are not intransitive
/// </summary>
/// <remarks>
/// This is a synthetic type, and not part of the ActivityStreams standard.
/// </remarks>
public class ASTransitiveActivity : ASActivity
{
    [JsonConstructor]
    public ASTransitiveActivity() {}
    public ASTransitiveActivity(string type) : base(type) {}

    /// <summary>
    /// Describes the direct object of the activity.
    /// For instance, in the activity "John added a movie to his wishlist", the object of the activity is the movie added. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-object"/>
    [JsonPropertyName("object")]
    public LinkableList<ASObject> Object { get; set; } = new();
    
    [CustomJsonDeserializer]
    public static bool TryDeserialize(JsonElement element, JsonSerializerOptions options, out ASTransitiveActivity? obj)
    {
        // If it has the "target" property, then its Targeted.
        // Pivot to the narrower type.
        if (element.TryGetProperty("target", out _))
        {
            obj = element.Deserialize<ASTargetedActivity>(options);
            return true;
        }

        // Otherwise we fall back on default
        obj = null;
        return false;
    }
}