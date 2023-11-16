// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a Web Page.
/// </summary>
public class PageObject : DocumentObject, IASModel<PageObject, PageObjectEntity, DocumentObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Page" types.
    /// </summary>
    public const string PageType = "Page";
    static string IASModel<PageObject>.ASTypeName => PageType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public PageObject() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public PageObject(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<PageObjectEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public PageObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public PageObject(TypeMap typeMap, PageObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<PageObjectEntity>();

    static PageObject IASModel<PageObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private PageObjectEntity Entity { get; }
}

/// <inheritdoc cref="PageObject" />
public sealed class PageObjectEntity : ASEntity<PageObject, PageObjectEntity> {}