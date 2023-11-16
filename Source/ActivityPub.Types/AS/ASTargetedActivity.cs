// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.AS;

/// <summary>
///     Synthetic base for activities which require a target.
/// </summary>
public class ASTargetedActivity : ASTransitiveActivity, IASModel<ASTargetedActivity, ASTargetedActivityEntity, ASTransitiveActivity>
{
    public ASTargetedActivity() : this(new TypeMap()) {}

    public ASTargetedActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new ASTargetedActivityEntity();
        TypeMap.AddEntity(Entity);
    }

    public ASTargetedActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public ASTargetedActivity(TypeMap typeMap, ASTargetedActivityEntity? entity) : base(typeMap, null)
    {
        Entity = entity ?? typeMap.AsEntity<ASTargetedActivityEntity>();
        Target = Entity.Target;
    }

    static ASTargetedActivity IASModel<ASTargetedActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

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
public sealed class ASTargetedActivityEntity : ASEntity<ASTargetedActivity, ASTargetedActivityEntity>
{
    /// <inheritdoc cref="ASTargetedActivity.Target" />
    [JsonPropertyName("target")]
    public LinkableList<ASObject> Target { get; set; } = new();
}