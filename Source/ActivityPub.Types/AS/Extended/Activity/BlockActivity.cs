// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is blocking the object.
///     Blocking is a stronger form of Ignore.
///     The typical use is to support social systems that allow one user to block activities or content of other users.
///     The target and origin typically have no defined meaning.
/// </summary>
public class BlockActivity : IgnoreActivity, IASModel<BlockActivity, BlockActivityEntity, IgnoreActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Block" types.
    /// </summary>
    [PublicAPI]
    public const string BlockType = "Block";
    static string IASModel<BlockActivity>.ASTypeName => BlockType;

    /// <inheritdoc />
    public BlockActivity() => Entity = TypeMap.Extend<BlockActivity, BlockActivityEntity>();

    /// <inheritdoc />
    public BlockActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<BlockActivity, BlockActivityEntity>(isExtending);

    /// <inheritdoc />
    public BlockActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public BlockActivity(TypeMap typeMap, BlockActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<BlockActivity, BlockActivityEntity>();

    static BlockActivity IASModel<BlockActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private BlockActivityEntity Entity { get; }
}

/// <inheritdoc cref="BlockActivity" />
public sealed class BlockActivityEntity : ASEntity<BlockActivity, BlockActivityEntity> {}