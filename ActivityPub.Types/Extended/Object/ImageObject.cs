// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// An image document of any kind 
/// </summary>
public class ImageObject : DocumentObject
{
    private ImageObjectEntity Entity { get; }

    public ImageObject() => Entity = new ImageObjectEntity(TypeMap);
    public ImageObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ImageObjectEntity>();
}

/// <inheritdoc cref="ImageObject"/>
[ASTypeKey(ImageType)]
[ImpliesOtherEntity(typeof(DocumentObjectEntity))]
public sealed class ImageObjectEntity : ASBase
{
    public const string ImageType = "Image";

    /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public ImageObjectEntity(TypeMap typeMap) : base(ImageType, typeMap) {}

    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public ImageObjectEntity() : base(ImageType) {}
}