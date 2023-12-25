// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS;

/// <summary>
///     An Activity is a subtype of Object that describes some form of action that may happen, is currently happening, or has already happened.
///     The Activity type itself serves as an abstract base type for all types of activities.
///     It is important to note that the Activity type itself does not carry any specific semantics about the kind of action being taken.
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-activity" />
public class ASActivity : ASObject, IASModel<ASActivity, ASActivityEntity, ASObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Activity" types.
    /// </summary>
    [PublicAPI]
    public const string ActivityType = "Activity";
    static string IASModel<ASActivity>.ASTypeName => ActivityType;

    /// <inheritdoc />
    public ASActivity() => Entity = TypeMap.Extend<ASActivity, ASActivityEntity>();

    /// <inheritdoc />
    public ASActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<ASActivity, ASActivityEntity>(isExtending);

    /// <inheritdoc />
    public ASActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public ASActivity(TypeMap typeMap, ASActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ASActivity, ASActivityEntity>();

    static ASActivity IASModel<ASActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ASActivityEntity Entity { get; }

    /// <summary>
    ///     Describes one or more entities that either performed or are expected to perform the activity.
    ///     Any single activity can have multiple actors.
    ///     The actor MAY be specified using an indirect Link.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-actor" />
    public LinkableList<ASObject> Actor
    {
        get => Entity.Actor;
        set => Entity.Actor = value;
    }

    /// <summary>
    ///     Identifies one or more objects used (or to be used) in the completion of an Activity.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-instrument" />
    public LinkableList<ASObject> Instrument
    {
        get => Entity.Instrument;
        set => Entity.Instrument = value;
    }

    /// <summary>
    ///     Describes the direct object of the activity.
    ///     For instance, in the activity "John added a movie to his wishlist", the object of the activity is the movie added.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-object" />
    public LinkableList<ASObject> Object
    {
        get => Entity.Object;
        set => Entity.Object = value;
    }
    
    /// <summary>
    ///     Describes an indirect object of the activity from which the activity is directed.
    ///     The precise meaning of the origin is the object of the English preposition "from".
    ///     For instance, in the activity "John moved an item to List B from List A", the origin of the activity is "List A".
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-origin" />
    public LinkableList<ASObject> Origin
    {
        get => Entity.Origin;
        set => Entity.Origin = value;
    }

    /// <summary>
    ///     Describes the result of the activity.
    ///     For instance, if a particular action results in the creation of a new resource, the result property can be used to describe that new resource.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-result" />
    public LinkableList<ASObject> Result
    {
        get => Entity.Result;
        set => Entity.Result = value;
    }

    /// <summary>
    ///     Describes the indirect object, or target, of the activity.
    ///     The precise meaning of the target is largely dependent on the type of action being described but will often be the object of the English preposition "to".
    ///     For instance, in the activity "John added a movie to his wishlist", the target of the activity is John's wishlist.
    ///     An activity can have more than one target.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-target" />
    /// <seealso href="https://www.w3.org/TR/activitypub/#client-addressing" />
    public LinkableList<ASObject> Target
    {
        get => Entity.Target;
        set => Entity.Target = value;
    }
}

/// <inheritdoc cref="ASActivity" />
public sealed class ASActivityEntity : ASEntity<ASActivity, ASActivityEntity>
{
    /// <inheritdoc cref="ASActivity.Actor" />
    [JsonPropertyName("actor")]
    public LinkableList<ASObject> Actor { get; set; } = new();

    /// <inheritdoc cref="ASActivity.Instrument" />
    [JsonPropertyName("instrument")]
    public LinkableList<ASObject> Instrument { get; set; } = new();
    
    /// <inheritdoc cref="ASActivity.Object" />
    [JsonPropertyName("object")]
    public LinkableList<ASObject> Object { get; set; } = new();

    /// <inheritdoc cref="ASActivity.Origin" />
    [JsonPropertyName("origin")]
    public LinkableList<ASObject> Origin { get; set; } = new();

    /// <inheritdoc cref="ASActivity.Result" />
    [JsonPropertyName("result")]
    public LinkableList<ASObject> Result { get; set; } = new();
    
    /// <inheritdoc cref="ASActivity.Target" />
    [JsonPropertyName("target")]
    public LinkableList<ASObject> Target { get; set; } = new();
}