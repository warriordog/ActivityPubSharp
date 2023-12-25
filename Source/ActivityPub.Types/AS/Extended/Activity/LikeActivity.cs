// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor likes, recommends or endorses the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class LikeActivity : ASActivity, IASModel<LikeActivity, LikeActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Like" types.
    /// </summary>
    [PublicAPI]
    public const string LikeType = "Like";
    static string IASModel<LikeActivity>.ASTypeName => LikeType;

    /// <inheritdoc />
    public LikeActivity() => Entity = TypeMap.Extend<LikeActivity, LikeActivityEntity>();

    /// <inheritdoc />
    public LikeActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<LikeActivity, LikeActivityEntity>(isExtending);

    /// <inheritdoc />
    public LikeActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public LikeActivity(TypeMap typeMap, LikeActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<LikeActivity, LikeActivityEntity>();

    static LikeActivity IASModel<LikeActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private LikeActivityEntity Entity { get; }
}

/// <inheritdoc cref="LikeActivity" />
public sealed class LikeActivityEntity : ASEntity<LikeActivity, LikeActivityEntity> {}