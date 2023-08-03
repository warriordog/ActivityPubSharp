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
public sealed class PageObjectEntity : ASBase<PageObject>
{
    public const string PageType = "Page";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        DocumentObjectEntity.DocumentType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public PageObjectEntity(TypeMap typeMap) : base(typeMap, PageType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public PageObjectEntity() : base(PageType, ReplacedTypes) {}
}