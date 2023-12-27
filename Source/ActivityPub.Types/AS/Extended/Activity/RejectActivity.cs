// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is rejecting the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class RejectActivity : ASActivity, IASModel<RejectActivity, RejectActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Reject" types.
    /// </summary>
    [PublicAPI]
    public const string RejectType = "Reject";
    static string IASModel<RejectActivity>.ASTypeName => RejectType;

    /// <inheritdoc />
    public RejectActivity() => Entity = TypeMap.Extend<RejectActivity, RejectActivityEntity>();

    /// <inheritdoc />
    public RejectActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<RejectActivity, RejectActivityEntity>(isExtending);

    /// <inheritdoc />
    public RejectActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public RejectActivity(TypeMap typeMap, RejectActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<RejectActivity, RejectActivityEntity>();

    static RejectActivity IASModel<RejectActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private RejectActivityEntity Entity { get; }
}

/// <inheritdoc cref="RejectActivity" />
public sealed class RejectActivityEntity : ASEntity<RejectActivity, RejectActivityEntity> {}