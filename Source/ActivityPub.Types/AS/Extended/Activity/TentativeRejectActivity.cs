// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     A specialization of Reject in which the rejection is considered tentative.
/// </summary>
public class TentativeRejectActivity : RejectActivity, IASModel<TentativeRejectActivity, TentativeRejectActivityEntity, RejectActivity>
{
    public const string TentativeRejectType = "TentativeReject";
    static string IASModel<TentativeRejectActivity>.ASTypeName => TentativeRejectType;

    public TentativeRejectActivity() : this(new TypeMap()) {}

    public TentativeRejectActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<TentativeRejectActivityEntity>();

    public TentativeRejectActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public TentativeRejectActivity(TypeMap typeMap, TentativeRejectActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<TentativeRejectActivityEntity>();

    static TentativeRejectActivity IASModel<TentativeRejectActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private TentativeRejectActivityEntity Entity { get; }
}

/// <inheritdoc cref="TentativeRejectActivity" />
public sealed class TentativeRejectActivityEntity : ASEntity<TentativeRejectActivity, TentativeRejectActivityEntity> {}