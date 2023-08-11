// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.AS;

/// <summary>
///     Synthetic base for activities which require a target.
/// </summary>
public class ASTargetedActivity : ASTransitiveActivity
{
    public ASTargetedActivity() => Entity = new ASTargetedActivityEntity
    {
        TypeMap = TypeMap,
        Target = null!
    };

    public ASTargetedActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ASTargetedActivityEntity>();
    private ASTargetedActivityEntity Entity { get; }


    /// <summary>
    ///     Describes the indirect object, or target, of the activity.
    ///     The precise meaning of the target is largely dependent on the type of action being described but will often be the object of the English preposition "to".
    ///     For instance, in the activity "John added a movie to his wishlist", the target of the activity is John's wishlist.
    ///     An activity can have more than one target.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-target" />
    /// <seealso href="https://www.w3.org/TR/activitypub/#client-addressing" />
    [JsonPropertyName("target")]
    public required LinkableList<ASObject> Target
    {
        get => Entity.Target;
        set => Entity.Target = value;
    }
}

/// <inheritdoc cref="ASTargetedActivity" />
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class ASTargetedActivityEntity : ASEntity<ASTargetedActivity>
{
    /// <inheritdoc cref="ASTargetedActivity.Target" />
    [JsonPropertyName("target")]
    public required LinkableList<ASObject> Target { get; set; }
}