// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is blocking the object.
/// Blocking is a stronger form of Ignore.
/// The typical use is to support social systems that allow one user to block activities or content of other users.
/// The target and origin typically have no defined meaning. 
/// </summary>
public class BlockActivity : IgnoreActivity
{
    private BlockActivityEntity Entity { get; }

    public BlockActivity() => Entity = new BlockActivityEntity(TypeMap);
    public BlockActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<BlockActivityEntity>();
}

/// <inheritdoc cref="BlockActivity"/>
[ASTypeKey(BlockType)]
[ImpliesOtherEntity(typeof(IgnoreActivityEntity))]
public sealed class BlockActivityEntity : ASBase
{
    public const string BlockType = "Block";

    /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public BlockActivityEntity(TypeMap typeMap) : base(BlockType, typeMap) {}

    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public BlockActivityEntity() : base(BlockType) {}
}