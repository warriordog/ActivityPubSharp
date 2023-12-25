// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents any kind of multi-paragraph written work.
/// </summary>
public class ArticleObject : ASObject, IASModel<ArticleObject, ArticleObjectEntity, ASObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Article" types.
    /// </summary>
    [PublicAPI]
    public const string ArticleType = "Article";
    static string IASModel<ArticleObject>.ASTypeName => ArticleType;

    /// <inheritdoc />
    public ArticleObject() => Entity = TypeMap.Extend<ArticleObject, ArticleObjectEntity>();

    /// <inheritdoc />
    public ArticleObject(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<ArticleObject, ArticleObjectEntity>(isExtending);

    /// <inheritdoc />
    public ArticleObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public ArticleObject(TypeMap typeMap, ArticleObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ArticleObject, ArticleObjectEntity>();

    static ArticleObject IASModel<ArticleObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private ArticleObjectEntity Entity { get; }
}

/// <inheritdoc cref="ArticleObject" />
public sealed class ArticleObjectEntity : ASEntity<ArticleObject, ArticleObjectEntity> {}