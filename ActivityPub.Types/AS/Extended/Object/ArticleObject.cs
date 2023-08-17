// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents any kind of multi-paragraph written work.
/// </summary>
public class ArticleObject : ASObject
{
    public ArticleObject() => Entity = new ArticleObjectEntity { TypeMap = TypeMap };
    public ArticleObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ArticleObjectEntity>();
    private ArticleObjectEntity Entity { get; }
}

/// <inheritdoc cref="ArticleObject" />
[APType(ArticleType)]
[ImpliesOtherEntity(typeof(ASObjectEntity))]
public sealed class ArticleObjectEntity : ASEntity<ArticleObject>
{
    public const string ArticleType = "Article";
    public override string ASTypeName => ArticleType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASObjectEntity.ObjectType
    };
}