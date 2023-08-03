// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is calling the target's attention the object.
/// The origin typically has no defined meaning. 
/// </summary>
public class AnnounceActivity : ASTransitiveActivity
{
    private AnnounceActivityEntity Entity { get; }

    public AnnounceActivity() => Entity = new AnnounceActivityEntity(TypeMap);
    public AnnounceActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<AnnounceActivityEntity>();
}

/// <inheritdoc cref="AnnounceActivity"/>
[ASTypeKey(AnnounceType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class AnnounceActivityEntity : ASBase<AnnounceActivity>
{
    public const string AnnounceType = "Announce";

    /// <inheritdoc cref="ASBase{T}(string?, TypeMap)"/>
    public AnnounceActivityEntity(TypeMap typeMap) : base(AnnounceType, typeMap) {}

    /// <inheritdoc cref="ASBase{T}(string?)"/>
    [JsonConstructor]
    public AnnounceActivityEntity() : base(AnnounceType) {}
}