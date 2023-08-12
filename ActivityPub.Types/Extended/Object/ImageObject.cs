// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// An image document of any kind 
/// </summary>
[APTypeAttribute(ImageType)]
public class ImageObject : DocumentObject
{
    public const string ImageType = "Image";

    [JsonConstructor]
    public ImageObject() : this(ImageType) {}

    protected ImageObject(string type) : base(type) {}
}
