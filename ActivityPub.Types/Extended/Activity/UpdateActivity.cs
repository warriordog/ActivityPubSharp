// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has updated the object.
/// Note, however, that this vocabulary does not define a mechanism for describing the actual set of modifications made to object.
/// The target and origin typically have no defined meaning. 
/// </summary>
public class UpdateActivity : ASTransitiveActivity
{
    private UpdateActivityEntity Entity { get; }

    public UpdateActivity() => Entity = new UpdateActivityEntity(TypeMap);
    public UpdateActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<UpdateActivityEntity>();
}

/// <inheritdoc cref="UpdateActivity"/>
[ASTypeKey(UpdateType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class UpdateActivityEntity : ASBase<UpdateActivity>
{
    public const string UpdateType = "Update";

    /// <inheritdoc cref="ASBase{T}(string?, TypeMap)"/>
    public UpdateActivityEntity(TypeMap typeMap) : base(UpdateType, typeMap) {}

    /// <inheritdoc cref="ASBase{T}(string?)"/>
    [JsonConstructor]
    public UpdateActivityEntity() : base(UpdateType) {}
}