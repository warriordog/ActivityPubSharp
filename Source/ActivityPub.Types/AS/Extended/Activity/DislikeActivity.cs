// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor dislikes the object.
/// </summary>
public class DislikeActivity : ASActivity, IASModel<DislikeActivity, DislikeActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Dislike" types.
    /// </summary>
    public const string DislikeType = "Dislike";
    static string IASModel<DislikeActivity>.ASTypeName => DislikeType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public DislikeActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public DislikeActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<DislikeActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public DislikeActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public DislikeActivity(TypeMap typeMap, DislikeActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<DislikeActivityEntity>();

    static DislikeActivity IASModel<DislikeActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private DislikeActivityEntity Entity { get; }
}

/// <inheritdoc cref="DislikeActivity" />
public sealed class DislikeActivityEntity : ASEntity<DislikeActivity, DislikeActivityEntity> {}