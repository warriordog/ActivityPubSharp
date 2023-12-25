// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     An image document of any kind
/// </summary>
public class ImageObject : DocumentObject, IASModel<ImageObject, ImageObjectEntity, DocumentObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Image" types.
    /// </summary>
    [PublicAPI]
    public const string ImageType = "Image";
    static string IASModel<ImageObject>.ASTypeName => ImageType;

    /// <inheritdoc />
    public ImageObject() => Entity = TypeMap.Extend<ImageObject, ImageObjectEntity>();

    /// <inheritdoc />
    public ImageObject(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<ImageObject, ImageObjectEntity>(isExtending);

    /// <inheritdoc />
    public ImageObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public ImageObject(TypeMap typeMap, ImageObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ImageObject, ImageObjectEntity>();

    static ImageObject IASModel<ImageObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private ImageObjectEntity Entity { get; }
}

/// <inheritdoc cref="ImageObject" />
public sealed class ImageObjectEntity : ASEntity<ImageObject, ImageObjectEntity> {}