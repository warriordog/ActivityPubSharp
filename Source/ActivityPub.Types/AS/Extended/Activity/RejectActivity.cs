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
    /// <summary>
    ///     ActivityStreams type name for "Reject" types.
    /// </summary>
    public const string RejectType = "Reject";
    static string IASModel<RejectActivity>.ASTypeName => RejectType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public RejectActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public RejectActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<RejectActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public RejectActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public RejectActivity(TypeMap typeMap, RejectActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<RejectActivityEntity>();

    static RejectActivity IASModel<RejectActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private RejectActivityEntity Entity { get; }
}

/// <inheritdoc cref="RejectActivity" />
public sealed class RejectActivityEntity : ASEntity<RejectActivity, RejectActivityEntity> {}