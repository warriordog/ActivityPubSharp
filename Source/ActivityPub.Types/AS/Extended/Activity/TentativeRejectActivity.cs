// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     A specialization of Reject in which the rejection is considered tentative.
/// </summary>
public class TentativeRejectActivity : RejectActivity, IASModel<TentativeRejectActivity, TentativeRejectActivityEntity, RejectActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "TentativeReject" types.
    /// </summary>
    public const string TentativeRejectType = "TentativeReject";
    static string IASModel<TentativeRejectActivity>.ASTypeName => TentativeRejectType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public TentativeRejectActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public TentativeRejectActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<TentativeRejectActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public TentativeRejectActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public TentativeRejectActivity(TypeMap typeMap, TentativeRejectActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<TentativeRejectActivityEntity>();

    static TentativeRejectActivity IASModel<TentativeRejectActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private TentativeRejectActivityEntity Entity { get; }
}

/// <inheritdoc cref="TentativeRejectActivity" />
public sealed class TentativeRejectActivityEntity : ASEntity<TentativeRejectActivity, TentativeRejectActivityEntity> {}