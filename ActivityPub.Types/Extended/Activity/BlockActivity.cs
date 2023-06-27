// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is blocking the object.
/// Blocking is a stronger form of Ignore.
/// The typical use is to support social systems that allow one user to block activities or content of other users.
/// The target and origin typically have no defined meaning. 
/// </summary>
[ASTypeKey(BlockType)]
public class BlockActivity : IgnoreActivity
{
    public const string BlockType = "BlockActivity";

    [JsonConstructor]
    public BlockActivity() : this(BlockType) {}

    protected BlockActivity(string type) : base(type) {}
}