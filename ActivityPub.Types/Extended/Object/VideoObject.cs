/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a video document of any kind. 
/// </summary>
public class VideoObject : DocumentObject
{
    public const string VideoType = "Video";
    public VideoObject(string type = VideoType) : base(type) {}
}