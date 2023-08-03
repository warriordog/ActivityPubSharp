// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// Represents any kind of multi-paragraph written work.
/// </summary>
public class ArticleObject : ASObject
{
    private ArticleObjectEntity Entity { get; }

    public ArticleObject() => Entity = new ArticleObjectEntity(TypeMap);
    public ArticleObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ArticleObjectEntity>();
}

/// <inheritdoc cref="ArticleObject"/>
[ASTypeKey(ArticleType)]
[ImpliesOtherEntity(typeof(ASObjectEntity))]
public sealed class ArticleObjectEntity : ASBase<ArticleObject>
{
    public const string ArticleType = "Article";

    /// <inheritdoc cref="ASBase{T}(string?, TypeMap)"/>
    public ArticleObjectEntity(TypeMap typeMap) : base(ArticleType, typeMap) {}

    /// <inheritdoc cref="ASBase{T}(string?)"/>
    [JsonConstructor]
    public ArticleObjectEntity() : base(ArticleType) {}
}