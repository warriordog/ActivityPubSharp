// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a video document of any kind. 
/// </summary>
public class VideoObject : DocumentObject
{
    private VideoObjectEntity Entity { get; }

    public VideoObject() => Entity = new VideoObjectEntity { TypeMap = TypeMap };
    public VideoObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<VideoObjectEntity>();
}

/// <inheritdoc cref="VideoObject"/>
[ASTypeKey(VideoType)]
[ImpliesOtherEntity(typeof(DocumentObjectEntity))]
public sealed class VideoObjectEntity : ASBase<VideoObject>
{
    public const string VideoType = "Video";
    public override string ASTypeName => VideoType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>()
    {
        DocumentObjectEntity.DocumentType
    };
}