// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     A specialization of TentativeAccept indicating that the tentativeAcceptance is tentative.
/// </summary>
public class TentativeAcceptActivity : AcceptActivity, IASModel<TentativeAcceptActivity, TentativeAcceptActivityEntity, AcceptActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "TentativeAccept" types.
    /// </summary>
    public const string TentativeAcceptType = "TentativeAccept";
    static string IASModel<TentativeAcceptActivity>.ASTypeName => TentativeAcceptType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public TentativeAcceptActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public TentativeAcceptActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<TentativeAcceptActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public TentativeAcceptActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public TentativeAcceptActivity(TypeMap typeMap, TentativeAcceptActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<TentativeAcceptActivityEntity>();

    static TentativeAcceptActivity IASModel<TentativeAcceptActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private TentativeAcceptActivityEntity Entity { get; }
}

/// <inheritdoc cref="TentativeAcceptActivity" />
public sealed class TentativeAcceptActivityEntity : ASEntity<TentativeAcceptActivity, TentativeAcceptActivityEntity> {}