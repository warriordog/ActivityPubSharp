// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor likes, recommends or endorses the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class LikeActivity : ASTransitiveActivity, IASModel<LikeActivity, LikeActivityEntity, ASTransitiveActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Like" types.
    /// </summary>
    public const string LikeType = "Like";
    static string IASModel<LikeActivity>.ASTypeName => LikeType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public LikeActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public LikeActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<LikeActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public LikeActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public LikeActivity(TypeMap typeMap, LikeActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<LikeActivityEntity>();

    static LikeActivity IASModel<LikeActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private LikeActivityEntity Entity { get; }
}

/// <inheritdoc cref="LikeActivity" />
public sealed class LikeActivityEntity : ASEntity<LikeActivity, LikeActivityEntity> {}