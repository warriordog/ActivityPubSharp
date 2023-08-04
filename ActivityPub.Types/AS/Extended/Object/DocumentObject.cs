// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a document of any kind.
/// </summary>
public class DocumentObject : ASObject
{
    public DocumentObject() => Entity = new DocumentObjectEntity { TypeMap = TypeMap };
    public DocumentObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<DocumentObjectEntity>();
    private DocumentObjectEntity Entity { get; }
}

/// <inheritdoc cref="DocumentObject" />
[ASTypeKey(DocumentType)]
[ImpliesOtherEntity(typeof(ASObjectEntity))]
public sealed class DocumentObjectEntity : ASEntity<DocumentObject>
{
    public const string DocumentType = "Document";
    public override string ASTypeName => DocumentType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASObjectEntity.ObjectType
    };
}