// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a Web Page.
/// </summary>
public class PageObject : DocumentObject, IASModel<PageObject, PageObjectEntity, DocumentObject>
{
    public const string PageType = "Page";
    static string IASModel<PageObject>.ASTypeName => PageType;

    public PageObject() : this(new TypeMap()) {}

    public PageObject(TypeMap typeMap) : base(typeMap)
    {
        Entity = new PageObjectEntity();
        TypeMap.Add(Entity);
    }

    [SetsRequiredMembers]
    public PageObject(TypeMap typeMap, PageObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<PageObjectEntity>();

    static PageObject IASModel<PageObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private PageObjectEntity Entity { get; }
}

/// <inheritdoc cref="PageObject" />
public sealed class PageObjectEntity : ASEntity<PageObject, PageObjectEntity> {}