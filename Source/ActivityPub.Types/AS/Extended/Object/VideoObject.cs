// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a video document of any kind.
/// </summary>
public class VideoObject : DocumentObject, IASModel<VideoObject, VideoObjectEntity, DocumentObject>
{
    public const string VideoType = "Video";
    static string IASModel<VideoObject>.ASTypeName => VideoType;

    public VideoObject() : this(new TypeMap()) {}

    public VideoObject(TypeMap typeMap) : base(typeMap)
    {
        Entity = new VideoObjectEntity();
        TypeMap.AddEntity(Entity);
    }

    public VideoObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public VideoObject(TypeMap typeMap, VideoObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<VideoObjectEntity>();

    static VideoObject IASModel<VideoObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private VideoObjectEntity Entity { get; }
}

/// <inheritdoc cref="VideoObject" />
public sealed class VideoObjectEntity : ASEntity<VideoObject, VideoObjectEntity> {}