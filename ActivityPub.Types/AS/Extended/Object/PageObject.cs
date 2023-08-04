// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a Web Page.
/// </summary>
public class PageObject : DocumentObject
{
    public PageObject() => Entity = new PageObjectEntity { TypeMap = TypeMap };
    public PageObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<PageObjectEntity>();
    private PageObjectEntity Entity { get; }
}

/// <inheritdoc cref="PageObject" />
[ASTypeKey(PageType)]
[ImpliesOtherEntity(typeof(DocumentObjectEntity))]
public sealed class PageObjectEntity : ASBase<PageObject>
{
    public const string PageType = "Page";
    public override string ASTypeName => PageType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        DocumentObjectEntity.DocumentType
    };
}