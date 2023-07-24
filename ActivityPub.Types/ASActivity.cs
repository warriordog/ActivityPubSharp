// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
/// An Activity is a subtype of Object that describes some form of action that may happen, is currently happening, or has already happened.
/// The Activity type itself serves as an abstract base type for all types of activities.
/// It is important to note that the Activity type itself does not carry any specific semantics about the kind of action being taken. 
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-activity"/>
[ASTypeKey(ActivityType)]
[CustomJsonDeserializer(nameof(TryDeserialize))]
public class ASActivity : ASObject
{
    public const string ActivityType = "Activity";

    [JsonConstructor]
    public ASActivity() : this(ActivityType) {}

    protected ASActivity(string type) : base(type) {}

    /// <summary>
    /// Describes one or more entities that either performed or are expected to perform the activity.
    /// Any single activity can have multiple actors.
    /// The actor MAY be specified using an indirect Link. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-actor"/>
    [JsonPropertyName("actor")]
    public LinkableList<ASObject> Actor { get; set; } = new();

    /// <summary>
    /// Identifies one or more objects used (or to be used) in the completion of an Activity. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-instrument"/>
    [JsonPropertyName("instrument")]
    public Linkable<ASObject>? Instrument { get; set; }

    /// <summary>
    /// Describes an indirect object of the activity from which the activity is directed.
    /// The precise meaning of the origin is the object of the English preposition "from".
    /// For instance, in the activity "John moved an item to List B from List A", the origin of the activity is "List A". 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-origin"/>
    [JsonPropertyName("origin")]
    public Linkable<ASObject>? Origin { get; set; }

    /// <summary>
    /// Describes the result of the activity.
    /// For instance, if a particular action results in the creation of a new resource, the result property can be used to describe that new resource. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-result"/>
    [JsonPropertyName("result")]
    public Linkable<ASObject>? Result { get; set; }

    /// <summary>
    /// Describes the indirect object, or target, of the activity.
    /// The precise meaning of the target is largely dependent on the type of action being described but will often be the object of the English preposition "to".
    /// For instance, in the activity "John added a movie to his wishlist", the target of the activity is John's wishlist.
    /// An activity can have more than one target. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-target"/>
    [JsonPropertyName("target")]
    public Linkable<ASObject>? Target
    {
        get => TargetImpl;
        set => TargetImpl = value;
    }

    /// <summary>
    /// Overridden in <see cref="ASTargetedActivity"/> to ensure that both properties stay in sync
    /// </summary>
    protected virtual Linkable<ASObject>? TargetImpl { get; set; }

    public static bool TryDeserialize(JsonElement element, JsonSerializerOptions options, out ASActivity? obj)
    {
        // If it has the "object" property, then its Transitive.
        // Pivot to the narrower type.
        if (element.TryGetProperty("object", out _))
        {
            obj = element.Deserialize<ASTransitiveActivity>(options);
            return true;
        }

        // Otherwise we fall back on default
        obj = null;
        return false;
    }
}