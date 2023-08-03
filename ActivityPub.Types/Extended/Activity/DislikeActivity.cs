// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

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
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class DislikeActivityEntity : ASBase<DislikeActivity>
{
    public const string DislikeType = "Dislike";

    /// <inheritdoc cref="ASBase{T}(string?, TypeMap)"/>
    public DislikeActivityEntity(TypeMap typeMap) : base(DislikeType, typeMap) {}

    /// <inheritdoc cref="ASBase{T}(string?)"/>
    [JsonConstructor]
    public DislikeActivityEntity() : base(DislikeType) {}
}