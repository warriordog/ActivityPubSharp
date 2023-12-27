// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has left the object.
///     The target and origin typically have no meaning.
/// </summary>
public class LeaveActivity : ASActivity, IASModel<LeaveActivity, LeaveActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Leave" types.
    /// </summary>
    [PublicAPI]
    public const string LeaveType = "Leave";
    static string IASModel<LeaveActivity>.ASTypeName => LeaveType;

    /// <inheritdoc />
    public LeaveActivity() => Entity = TypeMap.Extend<LeaveActivity, LeaveActivityEntity>();

    /// <inheritdoc />
    public LeaveActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<LeaveActivity, LeaveActivityEntity>(isExtending);

    /// <inheritdoc />
    public LeaveActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public LeaveActivity(TypeMap typeMap, LeaveActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<LeaveActivity, LeaveActivityEntity>();

    static LeaveActivity IASModel<LeaveActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private LeaveActivityEntity Entity { get; }
}

/// <inheritdoc cref="LeaveActivity" />
public sealed class LeaveActivityEntity : ASEntity<LeaveActivity, LeaveActivityEntity> {}