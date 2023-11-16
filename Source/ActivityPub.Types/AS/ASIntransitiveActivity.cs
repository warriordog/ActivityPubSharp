// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS;

/// <summary>
///     Instances of IntransitiveActivity are a subtype of Activity representing intransitive actions.
///     The object property is therefore inappropriate for these activities.
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-intransitiveactivity" />
public class ASIntransitiveActivity : ASActivity, IASModel<ASIntransitiveActivity, ASIntransitiveActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "IntransitiveActivity" types.
    /// </summary>
    public const string IntransitiveActivityType = "IntransitiveActivity";
    static string IASModel<ASIntransitiveActivity>.ASTypeName => IntransitiveActivityType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public ASIntransitiveActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ASIntransitiveActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<ASIntransitiveActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ASIntransitiveActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public ASIntransitiveActivity(TypeMap typeMap, ASIntransitiveActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ASIntransitiveActivityEntity>();

    static ASIntransitiveActivity IASModel<ASIntransitiveActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ASIntransitiveActivityEntity Entity { get; }
}

/// <inheritdoc cref="ASIntransitiveActivity" />
public sealed class ASIntransitiveActivityEntity : ASEntity<ASIntransitiveActivity, ASIntransitiveActivityEntity> {}