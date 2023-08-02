// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is ignoring the object. The target and origin typically have no defined meaning. 
/// </summary>
public class IgnoreActivity : ASTransitiveActivity
{
    private IgnoreActivityEntity Entity { get; }
    
    public IgnoreActivity() => Entity = new IgnoreActivityEntity(TypeMap);
    public IgnoreActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<IgnoreActivityEntity>();
}


/// <inheritdoc cref="IgnoreActivity"/>
[ASTypeKey(IgnoreType)]
public sealed class IgnoreActivityEntity : ASBase
{
    public const string IgnoreType = "Ignore";

        /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public IgnoreActivityEntity(TypeMap typeMap) : base(IgnoreType, typeMap) {}
    
    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public IgnoreActivityEntity() : base(IgnoreType) {}
}