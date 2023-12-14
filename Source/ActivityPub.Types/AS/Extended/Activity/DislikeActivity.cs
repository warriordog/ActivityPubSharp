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

    /// <inheritdoc />
    public DislikeActivity() => Entity = TypeMap.Extend<DislikeActivityEntity>();

    /// <inheritdoc />
    public DislikeActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<DislikeActivityEntity>(isExtending);

    /// <inheritdoc />
    public DislikeActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public DislikeActivity(TypeMap typeMap, DislikeActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<DislikeActivityEntity>();

    static DislikeActivity IASModel<DislikeActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private DislikeActivityEntity Entity { get; }
}

/// <inheritdoc cref="DislikeActivity" />
public sealed class DislikeActivityEntity : ASEntity<DislikeActivity, DislikeActivityEntity> {}