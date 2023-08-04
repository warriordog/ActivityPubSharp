// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// An image document of any kind 
/// </summary>
public class ImageObject : DocumentObject
{
    private ImageObjectEntity Entity { get; }

    public ImageObject() => Entity = new ImageObjectEntity { TypeMap = TypeMap };
    public ImageObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ImageObjectEntity>();
}

/// <inheritdoc cref="ImageObject"/>
[ASTypeKey(ImageType)]
[ImpliesOtherEntity(typeof(DocumentObjectEntity))]
public sealed class ImageObjectEntity : ASBase<ImageObject>
{
    public const string ImageType = "Image";
    public override string ASTypeName => ImageType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>()
    {
        DocumentObjectEntity.DocumentType
    };
}