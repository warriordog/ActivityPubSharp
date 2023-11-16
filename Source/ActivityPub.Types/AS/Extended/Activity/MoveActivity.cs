// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has moved object from origin to target.
///     If the origin or target are not specified, either can be determined by context.
/// </summary>
public class MoveActivity : ASTransitiveActivity, IASModel<MoveActivity, MoveActivityEntity, ASTransitiveActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Move" types.
    /// </summary>
    public const string MoveType = "Move";
    static string IASModel<MoveActivity>.ASTypeName => MoveType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public MoveActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public MoveActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<MoveActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public MoveActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public MoveActivity(TypeMap typeMap, MoveActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<MoveActivityEntity>();

    static MoveActivity IASModel<MoveActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private MoveActivityEntity Entity { get; }
}

/// <inheritdoc cref="MoveActivity" />
public sealed class MoveActivityEntity : ASEntity<MoveActivity, MoveActivityEntity> {}