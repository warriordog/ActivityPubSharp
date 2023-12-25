// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     A specialization of TentativeAccept indicating that the tentativeAcceptance is tentative.
/// </summary>
public class TentativeAcceptActivity : AcceptActivity, IASModel<TentativeAcceptActivity, TentativeAcceptActivityEntity, AcceptActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "TentativeAccept" types.
    /// </summary>
    [PublicAPI]
    public const string TentativeAcceptType = "TentativeAccept";
    static string IASModel<TentativeAcceptActivity>.ASTypeName => TentativeAcceptType;

    /// <inheritdoc />
    public TentativeAcceptActivity() => Entity = TypeMap.Extend<TentativeAcceptActivity, TentativeAcceptActivityEntity>();

    /// <inheritdoc />
    public TentativeAcceptActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<TentativeAcceptActivity, TentativeAcceptActivityEntity>(isExtending);

    /// <inheritdoc />
    public TentativeAcceptActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public TentativeAcceptActivity(TypeMap typeMap, TentativeAcceptActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<TentativeAcceptActivity, TentativeAcceptActivityEntity>();

    static TentativeAcceptActivity IASModel<TentativeAcceptActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private TentativeAcceptActivityEntity Entity { get; }
}

/// <inheritdoc cref="TentativeAcceptActivity" />
public sealed class TentativeAcceptActivityEntity : ASEntity<TentativeAcceptActivity, TentativeAcceptActivityEntity> {}