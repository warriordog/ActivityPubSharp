// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

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
public sealed class VideoObjectEntity : ASBase
{
    public const string VideoType = "Video";

    public VideoObjectEntity(TypeMap typeMap) : base(VideoType, typeMap) {}
}