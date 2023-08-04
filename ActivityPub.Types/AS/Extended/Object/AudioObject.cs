// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents an audio document of any kind.
/// </summary>
public class AudioObject : DocumentObject
{
    public AudioObject() => Entity = new AudioObjectEntity { TypeMap = TypeMap };
    public AudioObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<AudioObjectEntity>();
    private AudioObjectEntity Entity { get; }
}

/// <inheritdoc cref="AudioObject" />
[ASTypeKey(AudioType)]
[ImpliesOtherEntity(typeof(DocumentObjectEntity))]
public sealed class AudioObjectEntity : ASEntity<AudioObject>
{
    public const string AudioType = "Audio";
    public override string ASTypeName => AudioType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        DocumentObjectEntity.DocumentType
    };
}