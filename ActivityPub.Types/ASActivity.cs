// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;
using ActivityPub.Types.Conversion;
using ActivityPub.Types.Json.Attributes;
using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
/// An Activity is a subtype of Object that describes some form of action that may happen, is currently happening, or has already happened.
/// The Activity type itself serves as an abstract base type for all types of activities.
/// It is important to note that the Activity type itself does not carry any specific semantics about the kind of action being taken. 
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-activity"/>
public class ASActivity : ASObject
{
    private ASActivityEntity Entity { get; }


    public ASActivity() => Entity = new ASActivityEntity(TypeMap);
    public ASActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ASActivityEntity>();


    /// <summary>
    /// Describes one or more entities that either performed or are expected to perform the activity.
    /// Any single activity can have multiple actors.
    /// The actor MAY be specified using an indirect Link.
    /// TODO see if we can narrow the type to ASActor, after the changes
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-actor"/>
    public LinkableList<ASObject> Actor
    {
        get => Entity.Actor;
        set => Entity.Actor = value;
    }

    /// <summary>
    /// Identifies one or more objects used (or to be used) in the completion of an Activity. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-instrument"/>
    public Linkable<ASObject>? Instrument
    {
        get => Entity.Instrument;
        set => Entity.Instrument = value;
    }

    /// <summary>
    /// Describes an indirect object of the activity from which the activity is directed.
    /// The precise meaning of the origin is the object of the English preposition "from".
    /// For instance, in the activity "John moved an item to List B from List A", the origin of the activity is "List A". 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-origin"/>
    public Linkable<ASObject>? Origin
    {
        get => Entity.Origin;
        set => Entity.Origin = value;
    }

    /// <summary>
    /// Describes the result of the activity.
    /// For instance, if a particular action results in the creation of a new resource, the result property can be used to describe that new resource. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-result"/>
    public Linkable<ASObject>? Result
    {
        get => Entity.Result;
        set => Entity.Result = value;
    }
}

/// <inheritdoc cref="ASActivity"/>
[ASTypeKey(ActivityType)]
[ImpliesOtherEntity(typeof(ASObjectEntity))]
[NarrowJsonType(nameof(NarrowType))]
public sealed class ASActivityEntity : ASBase
{
    public const string ActivityType = "Activity";


    /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public ASActivityEntity(TypeMap typeMap) : base(ActivityType, typeMap) {}

    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public ASActivityEntity() : base(ActivityType) {}


    /// <inheritdoc cref="ASActivity.Actor"/>
    [JsonPropertyName("actor")]
    public LinkableList<ASObject> Actor { get; set; } = new();

    /// <inheritdoc cref="ASActivity.Instrument"/>
    [JsonPropertyName("instrument")]
    public Linkable<ASObject>? Instrument { get; set; }

    /// <inheritdoc cref="ASActivity.Origin"/>
    [JsonPropertyName("origin")]
    public Linkable<ASObject>? Origin { get; set; }

    /// <inheritdoc cref="ASActivity.Result"/>
    [JsonPropertyName("result")]
    public Linkable<ASObject>? Result { get; set; }

    /// <inheritdoc cref="NarrowTypeDelegate"/>
    public static Type NarrowType(JsonElement element, DeserializationMetadata meta)
    {
        // If it has the "object" property, then its Transitive.
        var isTransient = element.TryGetProperty("object", out _);

        // Pivot to the correct type.
        return isTransient
            ? typeof(ASTransitiveActivityEntity)
            : typeof(ASActivityEntity);
    }
}