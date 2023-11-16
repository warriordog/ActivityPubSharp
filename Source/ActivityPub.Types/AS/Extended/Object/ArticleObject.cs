// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents any kind of multi-paragraph written work.
/// </summary>
public class ArticleObject : ASObject, IASModel<ArticleObject, ArticleObjectEntity, ASObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Article" types.
    /// </summary>
    public const string ArticleType = "Article";
    static string IASModel<ArticleObject>.ASTypeName => ArticleType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public ArticleObject() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ArticleObject(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<ArticleObjectEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ArticleObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public ArticleObject(TypeMap typeMap, ArticleObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ArticleObjectEntity>();

    static ArticleObject IASModel<ArticleObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private ArticleObjectEntity Entity { get; }
}

/// <inheritdoc cref="ArticleObject" />
public sealed class ArticleObjectEntity : ASEntity<ArticleObject, ArticleObjectEntity> {}