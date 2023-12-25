// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has moved object from origin to target.
///     If the origin or target are not specified, either can be determined by context.
/// </summary>
public class MoveActivity : ASActivity, IASModel<MoveActivity, MoveActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Move" types.
    /// </summary>
    [PublicAPI]
    public const string MoveType = "Move";
    static string IASModel<MoveActivity>.ASTypeName => MoveType;

    /// <inheritdoc />
    public MoveActivity() => Entity = TypeMap.Extend<MoveActivity, MoveActivityEntity>();

    /// <inheritdoc />
    public MoveActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<MoveActivity, MoveActivityEntity>(isExtending);

    /// <inheritdoc />
    public MoveActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public MoveActivity(TypeMap typeMap, MoveActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<MoveActivity, MoveActivityEntity>();

    static MoveActivity IASModel<MoveActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private MoveActivityEntity Entity { get; }
}

/// <inheritdoc cref="MoveActivity" />
public sealed class MoveActivityEntity : ASEntity<MoveActivity, MoveActivityEntity> {}