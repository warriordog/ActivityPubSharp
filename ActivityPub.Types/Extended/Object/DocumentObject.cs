// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a document of any kind. 
/// </summary>
public class DocumentObject : ASObject
{
    private DocumentObjectEntity Entity { get; }

    public DocumentObject() => Entity = new DocumentObjectEntity(TypeMap);
    public DocumentObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<DocumentObjectEntity>();
}

/// <inheritdoc cref="DocumentObject"/>
[ASTypeKey(DocumentType)]
[ImpliesOtherEntity(typeof(ASObjectEntity))]
public sealed class DocumentObjectEntity : ASBase<DocumentObject>
{
    public const string DocumentType = "Document";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASObjectEntity.ObjectType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public DocumentObjectEntity(TypeMap typeMap) : base(typeMap, DocumentType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public DocumentObjectEntity() : base(DocumentType, ReplacedTypes) {}
}