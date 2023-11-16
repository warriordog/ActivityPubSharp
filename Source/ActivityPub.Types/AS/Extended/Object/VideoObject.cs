// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a video document of any kind.
/// </summary>
public class VideoObject : DocumentObject, IASModel<VideoObject, VideoObjectEntity, DocumentObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Video" types.
    /// </summary>
    public const string VideoType = "Video";
    static string IASModel<VideoObject>.ASTypeName => VideoType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public VideoObject() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public VideoObject(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<VideoObjectEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public VideoObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public VideoObject(TypeMap typeMap, VideoObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<VideoObjectEntity>();

    static VideoObject IASModel<VideoObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private VideoObjectEntity Entity { get; }
}

/// <inheritdoc cref="VideoObject" />
public sealed class VideoObjectEntity : ASEntity<VideoObject, VideoObjectEntity> {}