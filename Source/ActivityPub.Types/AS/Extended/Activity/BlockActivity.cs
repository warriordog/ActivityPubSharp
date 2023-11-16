// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is blocking the object.
///     Blocking is a stronger form of Ignore.
///     The typical use is to support social systems that allow one user to block activities or content of other users.
///     The target and origin typically have no defined meaning.
/// </summary>
public class BlockActivity : IgnoreActivity, IASModel<BlockActivity, BlockActivityEntity, IgnoreActivity>
{
    public const string BlockType = "Block";
    static string IASModel<BlockActivity>.ASTypeName => BlockType;

    public BlockActivity() : this(new TypeMap()) {}

    public BlockActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<BlockActivityEntity>();

    public BlockActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public BlockActivity(TypeMap typeMap, BlockActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<BlockActivityEntity>();

    static BlockActivity IASModel<BlockActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private BlockActivityEntity Entity { get; }
}

/// <inheritdoc cref="BlockActivity" />
public sealed class BlockActivityEntity : ASEntity<BlockActivity, BlockActivityEntity> {}