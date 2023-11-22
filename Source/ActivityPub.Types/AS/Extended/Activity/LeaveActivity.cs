// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

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
    public const string LeaveType = "Leave";
    static string IASModel<LeaveActivity>.ASTypeName => LeaveType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public LeaveActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public LeaveActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<LeaveActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public LeaveActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public LeaveActivity(TypeMap typeMap, LeaveActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<LeaveActivityEntity>();

    static LeaveActivity IASModel<LeaveActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private LeaveActivityEntity Entity { get; }
}

/// <inheritdoc cref="LeaveActivity" />
public sealed class LeaveActivityEntity : ASEntity<LeaveActivity, LeaveActivityEntity> {}