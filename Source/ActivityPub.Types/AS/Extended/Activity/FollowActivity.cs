// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is "following" the object.
///     Following is defined in the sense typically used within Social systems in which the actor is interested in any activity performed by or on the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class FollowActivity : ASActivity, IASModel<FollowActivity, FollowActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Follow" types.
    /// </summary>
    [PublicAPI]
    public const string FollowType = "Follow";
    static string IASModel<FollowActivity>.ASTypeName => FollowType;

    /// <inheritdoc />
    public FollowActivity() => Entity = TypeMap.Extend<FollowActivity, FollowActivityEntity>();

    /// <inheritdoc />
    public FollowActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<FollowActivity, FollowActivityEntity>(isExtending);

    /// <inheritdoc />
    public FollowActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public FollowActivity(TypeMap typeMap, FollowActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<FollowActivity, FollowActivityEntity>();

    static FollowActivity IASModel<FollowActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private FollowActivityEntity Entity { get; }
}

/// <inheritdoc cref="FollowActivity" />
public sealed class FollowActivityEntity : ASEntity<FollowActivity, FollowActivityEntity> {}