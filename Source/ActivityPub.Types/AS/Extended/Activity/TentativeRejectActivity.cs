// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     A specialization of Reject in which the rejection is considered tentative.
/// </summary>
public class TentativeRejectActivity : RejectActivity, IASModel<TentativeRejectActivity, TentativeRejectActivityEntity, RejectActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "TentativeReject" types.
    /// </summary>
    [PublicAPI]
    public const string TentativeRejectType = "TentativeReject";
    static string IASModel<TentativeRejectActivity>.ASTypeName => TentativeRejectType;

    /// <inheritdoc />
    public TentativeRejectActivity() => Entity = TypeMap.Extend<TentativeRejectActivity, TentativeRejectActivityEntity>();

    /// <inheritdoc />
    public TentativeRejectActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<TentativeRejectActivity, TentativeRejectActivityEntity>(isExtending);

    /// <inheritdoc />
    public TentativeRejectActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public TentativeRejectActivity(TypeMap typeMap, TentativeRejectActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<TentativeRejectActivity, TentativeRejectActivityEntity>();

    static TentativeRejectActivity IASModel<TentativeRejectActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private TentativeRejectActivityEntity Entity { get; }
}

/// <inheritdoc cref="TentativeRejectActivity" />
public sealed class TentativeRejectActivityEntity : ASEntity<TentativeRejectActivity, TentativeRejectActivityEntity> {}