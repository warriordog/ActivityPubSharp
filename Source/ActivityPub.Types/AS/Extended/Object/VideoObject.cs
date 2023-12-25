// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a video document of any kind.
/// </summary>
public class VideoObject : DocumentObject, IASModel<VideoObject, VideoObjectEntity, DocumentObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Video" types.
    /// </summary>
    [PublicAPI]
    public const string VideoType = "Video";
    static string IASModel<VideoObject>.ASTypeName => VideoType;

    /// <inheritdoc />
    public VideoObject() => Entity = TypeMap.Extend<VideoObject, VideoObjectEntity>();

    /// <inheritdoc />
    public VideoObject(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<VideoObject, VideoObjectEntity>(isExtending);

    /// <inheritdoc />
    public VideoObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public VideoObject(TypeMap typeMap, VideoObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<VideoObject, VideoObjectEntity>();

    static VideoObject IASModel<VideoObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private VideoObjectEntity Entity { get; }
}

/// <inheritdoc cref="VideoObject" />
public sealed class VideoObjectEntity : ASEntity<VideoObject, VideoObjectEntity> {}