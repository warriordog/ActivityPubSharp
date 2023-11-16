// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents an audio document of any kind.
/// </summary>
public class AudioObject : DocumentObject, IASModel<AudioObject, AudioObjectEntity, DocumentObject>
{
    public const string AudioType = "Audio";
    static string IASModel<AudioObject>.ASTypeName => AudioType;

    public AudioObject() : this(new TypeMap()) {}

    public AudioObject(TypeMap typeMap) : base(typeMap)
    {
        Entity = new AudioObjectEntity();
        TypeMap.Add(Entity);
    }

    public AudioObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public AudioObject(TypeMap typeMap, AudioObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<AudioObjectEntity>();

    static AudioObject IASModel<AudioObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private AudioObjectEntity Entity { get; }
}

/// <inheritdoc cref="AudioObject" />
public sealed class AudioObjectEntity : ASEntity<AudioObject, AudioObjectEntity> {}