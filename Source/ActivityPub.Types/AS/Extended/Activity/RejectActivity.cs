// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is rejecting the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class RejectActivity : ASTransitiveActivity, IASModel<RejectActivity, RejectActivityEntity, ASTransitiveActivity>
{
    public const string RejectType = "Reject";
    static string IASModel<RejectActivity>.ASTypeName => RejectType;

    public RejectActivity() : this(new TypeMap()) {}

    public RejectActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new RejectActivityEntity();
        TypeMap.AddEntity(Entity);
    }

    public RejectActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public RejectActivity(TypeMap typeMap, RejectActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<RejectActivityEntity>();

    static RejectActivity IASModel<RejectActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private RejectActivityEntity Entity { get; }
}

/// <inheritdoc cref="RejectActivity" />
public sealed class RejectActivityEntity : ASEntity<RejectActivity, RejectActivityEntity> {}