// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a document of any kind.
/// </summary>
public class DocumentObject : ASObject, IASModel<DocumentObject, DocumentObjectEntity, ASObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Document" types.
    /// </summary>
    public const string DocumentType = "Document";
    static string IASModel<DocumentObject>.ASTypeName => DocumentType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public DocumentObject() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public DocumentObject(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<DocumentObjectEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public DocumentObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public DocumentObject(TypeMap typeMap, DocumentObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<DocumentObjectEntity>();

    static DocumentObject IASModel<DocumentObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private DocumentObjectEntity Entity { get; }
}

/// <inheritdoc cref="DocumentObject" />
public sealed class DocumentObjectEntity : ASEntity<DocumentObject, DocumentObjectEntity> {}