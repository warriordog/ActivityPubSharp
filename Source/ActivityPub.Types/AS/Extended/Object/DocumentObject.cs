// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a document of any kind.
/// </summary>
public class DocumentObject : ASObject, IASModel<DocumentObject, DocumentObjectEntity, ASObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Document" types.
    /// </summary>
    [PublicAPI]
    public const string DocumentType = "Document";
    static string IASModel<DocumentObject>.ASTypeName => DocumentType;

    /// <inheritdoc />
    public DocumentObject() => Entity = TypeMap.Extend<DocumentObject, DocumentObjectEntity>();

    /// <inheritdoc />
    public DocumentObject(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<DocumentObject, DocumentObjectEntity>(isExtending);

    /// <inheritdoc />
    public DocumentObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public DocumentObject(TypeMap typeMap, DocumentObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<DocumentObject, DocumentObjectEntity>();

    static DocumentObject IASModel<DocumentObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private DocumentObjectEntity Entity { get; }
}

/// <inheritdoc cref="DocumentObject" />
public sealed class DocumentObjectEntity : ASEntity<DocumentObject, DocumentObjectEntity> {}