// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a video document of any kind. 
/// </summary>
public class VideoObject : DocumentObject
{
    private VideoObjectEntity Entity { get; }

    public VideoObject() => Entity = new VideoObjectEntity(TypeMap);
    public VideoObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<VideoObjectEntity>();
}

/// <inheritdoc cref="VideoObject"/>
[ASTypeKey(VideoType)]
[ImpliesOtherEntity(typeof(DocumentObjectEntity))]
public sealed class VideoObjectEntity : ASBase<VideoObject>
{
    public const string VideoType = "Video";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        DocumentObjectEntity.DocumentType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public VideoObjectEntity(TypeMap typeMap) : base(typeMap, VideoType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public VideoObjectEntity() : base(VideoType, ReplacedTypes) {}
}