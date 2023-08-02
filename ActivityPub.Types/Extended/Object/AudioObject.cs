// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents an audio document of any kind. 
/// </summary>
public class AudioObject : DocumentObject
{
    private AudioObjectEntity Entity { get; }
    
    public AudioObject() => Entity = new AudioObjectEntity(TypeMap);
    public AudioObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<AudioObjectEntity>();
}

/// <inheritdoc cref="AudioObject"/>
[ASTypeKey(AudioType)]
public sealed class AudioObjectEntity : ASBase
{
    public const string AudioType = "Audio";

        /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public AudioObjectEntity(TypeMap typeMap) : base(AudioType, typeMap) {}
    
    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public AudioObjectEntity() : base(AudioType) {}
}