// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has deleted the object.
/// If specified, the origin indicates the context from which the object was deleted. 
/// </summary>
public class DeleteActivity : ASTransitiveActivity
{
    private DeleteActivityEntity Entity { get; }

    public DeleteActivity() => Entity = new DeleteActivityEntity(TypeMap);
    public DeleteActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<DeleteActivityEntity>();
}

/// <inheritdoc cref="DeleteActivity"/>
[ASTypeKey(DeleteType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class DeleteActivityEntity : ASBase
{
    public const string DeleteType = "Delete";

    /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public DeleteActivityEntity(TypeMap typeMap) : base(DeleteType, typeMap) {}

    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public DeleteActivityEntity() : base(DeleteType) {}
}