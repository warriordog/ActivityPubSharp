// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

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
[ImpliesOtherEntity(typeof(DocumentObjectEntity))]
public sealed class AudioObjectEntity : ASBase<AudioObject>
{
    public const string AudioType = "Audio";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        DocumentObjectEntity.DocumentType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public AudioObjectEntity(TypeMap typeMap) : base(typeMap, AudioType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public AudioObjectEntity() : base(AudioType, ReplacedTypes) {}
}