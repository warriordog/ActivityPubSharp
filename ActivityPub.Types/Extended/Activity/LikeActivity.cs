// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor likes, recommends or endorses the object.
/// The target and origin typically have no defined meaning.
/// </summary>
public class LikeActivity : ASTransitiveActivity
{
    private LikeActivityEntity Entity { get; }
    
    public LikeActivity() => Entity = new LikeActivityEntity(TypeMap);
    public LikeActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<LikeActivityEntity>();
}


/// <inheritdoc cref="LikeActivity"/>
[ASTypeKey(LikeType)]
public sealed class LikeActivityEntity : ASBase
{
    public const string LikeType = "Like";

        /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public LikeActivityEntity(TypeMap typeMap) : base(LikeType, typeMap) {}
    
    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public LikeActivityEntity() : base(LikeType) {}
}