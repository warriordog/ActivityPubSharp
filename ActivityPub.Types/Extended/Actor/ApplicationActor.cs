// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Actor;

/// <summary>
/// Describes a software application. 
/// </summary>
public class ApplicationActor : ASActor
{
    private ApplicationActorEntity Entity { get; }
    
    public ApplicationActor() => Entity = new ApplicationActorEntity(TypeMap);
    public ApplicationActor(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ApplicationActorEntity>();
}


/// <inheritdoc cref="ApplicationActor"/>
[ASTypeKey(ApplicationType)]
public sealed class ApplicationActorEntity : ASBase
{
    public const string ApplicationType = "Application";

        /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public ApplicationActorEntity(TypeMap typeMap) : base(ApplicationType, typeMap) {}
    
    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public ApplicationActorEntity() : base(ApplicationType) {}
}