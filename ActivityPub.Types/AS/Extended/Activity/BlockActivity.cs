// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is blocking the object.
///     Blocking is a stronger form of Ignore.
///     The typical use is to support social systems that allow one user to block activities or content of other users.
///     The target and origin typically have no defined meaning.
/// </summary>
public class BlockActivity : IgnoreActivity
{
    public BlockActivity() => Entity = new BlockActivityEntity { TypeMap = TypeMap };
    public BlockActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<BlockActivityEntity>();
    private BlockActivityEntity Entity { get; }
}

/// <inheritdoc cref="BlockActivity" />
[ASTypeKey(BlockType)]
[ImpliesOtherEntity(typeof(IgnoreActivityEntity))]
public sealed class BlockActivityEntity : ASBase<BlockActivity>
{
    public const string BlockType = "Block";
    public override string ASTypeName => BlockType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        IgnoreActivityEntity.IgnoreType
    };
}