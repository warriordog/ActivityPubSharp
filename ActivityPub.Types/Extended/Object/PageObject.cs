// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents a Web Page. 
/// </summary>
public class PageObject : DocumentObject
{
    private PageObjectEntity Entity { get; }

    public PageObject() => Entity = new PageObjectEntity(TypeMap);
    public PageObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<PageObjectEntity>();
}

/// <inheritdoc cref="PageObject"/>
[ASTypeKey(PageType)]
[ImpliesOtherEntity(typeof(DocumentObjectEntity))]
public sealed class PageObjectEntity : ASBase
{
    public const string PageType = "Page";

    /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public PageObjectEntity(TypeMap typeMap) : base(PageType, typeMap) {}

    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public PageObjectEntity() : base(PageType) {}
}