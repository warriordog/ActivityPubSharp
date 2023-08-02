// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has read the object. 
/// </summary>
public class ReadActivity : ASTransitiveActivity
{
    private ReadActivityEntity Entity { get; }
    
    public ReadActivity() => Entity = new ReadActivityEntity(TypeMap);
    public ReadActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ReadActivityEntity>();
}


/// <inheritdoc cref="ReadActivity"/>
[ASTypeKey(ReadType)]
public sealed class ReadActivityEntity : ASBase
{
    public const string ReadType = "Read";

        /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public ReadActivityEntity(TypeMap typeMap) : base(ReadType, typeMap) {}
    
    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public ReadActivityEntity() : base(ReadType) {}
}