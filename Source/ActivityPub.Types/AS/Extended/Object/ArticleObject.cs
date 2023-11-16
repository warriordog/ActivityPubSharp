// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents any kind of multi-paragraph written work.
/// </summary>
public class ArticleObject : ASObject, IASModel<ArticleObject, ArticleObjectEntity, ASObject>
{
    public const string ArticleType = "Article";
    static string IASModel<ArticleObject>.ASTypeName => ArticleType;

    public ArticleObject() : this(new TypeMap()) {}

    public ArticleObject(TypeMap typeMap) : base(typeMap)
    {
        Entity = new ArticleObjectEntity();
        TypeMap.Add(Entity);
    }

    public ArticleObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public ArticleObject(TypeMap typeMap, ArticleObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ArticleObjectEntity>();

    static ArticleObject IASModel<ArticleObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private ArticleObjectEntity Entity { get; }
}

/// <inheritdoc cref="ArticleObject" />
public sealed class ArticleObjectEntity : ASEntity<ArticleObject, ArticleObjectEntity> {}