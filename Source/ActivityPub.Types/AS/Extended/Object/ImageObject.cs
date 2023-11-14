// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     An image document of any kind
/// </summary>
public class ImageObject : DocumentObject, IASModel<ImageObject, ImageObjectEntity, DocumentObject>
{
    public const string ImageType = "Image";
    static string IASModel<ImageObject>.ASTypeName => ImageType;

    public ImageObject() : this(new TypeMap()) {}

    public ImageObject(TypeMap typeMap) : base(typeMap)
    {
        Entity = new ImageObjectEntity();
        TypeMap.Add(Entity);
    }

    [SetsRequiredMembers]
    public ImageObject(TypeMap typeMap, ImageObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ImageObjectEntity>();

    static ImageObject IASModel<ImageObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private ImageObjectEntity Entity { get; }
}

/// <inheritdoc cref="ImageObject" />
public sealed class ImageObjectEntity : ASEntity<ImageObject, ImageObjectEntity> {}