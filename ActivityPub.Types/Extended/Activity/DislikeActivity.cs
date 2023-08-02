// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor dislikes the object. 
/// </summary>
public class DislikeActivity : ASTransitiveActivity
{
    private DislikeActivityEntity Entity { get; }
    
    public DislikeActivity() => Entity = new DislikeActivityEntity(TypeMap);
    public DislikeActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<DislikeActivityEntity>();
}


/// <inheritdoc cref="DislikeActivity"/>
[ASTypeKey(DislikeType)]
public sealed class DislikeActivityEntity : ASBase
{
    public const string DislikeType = "Dislike";

        /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public DislikeActivityEntity(TypeMap typeMap) : base(DislikeType, typeMap) {}
    
    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public DislikeActivityEntity() : base(DislikeType) {}
}