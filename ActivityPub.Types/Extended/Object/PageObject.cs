// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

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
public sealed class PageObjectEntity : ASBase
{
    public const string PageType = "Page";

    public PageObjectEntity(TypeMap typeMap) : base(PageType, typeMap) {}
}