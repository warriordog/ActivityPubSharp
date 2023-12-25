// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents a Web Page.
/// </summary>
public class PageObject : DocumentObject, IASModel<PageObject, PageObjectEntity, DocumentObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Page" types.
    /// </summary>
    [PublicAPI]
    public const string PageType = "Page";
    static string IASModel<PageObject>.ASTypeName => PageType;

    /// <inheritdoc />
    public PageObject() => Entity = TypeMap.Extend<PageObject, PageObjectEntity>();

    /// <inheritdoc />
    public PageObject(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<PageObject, PageObjectEntity>(isExtending);

    /// <inheritdoc />
    public PageObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public PageObject(TypeMap typeMap, PageObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<PageObject, PageObjectEntity>();

    static PageObject IASModel<PageObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private PageObjectEntity Entity { get; }
}

/// <inheritdoc cref="PageObject" />
public sealed class PageObjectEntity : ASEntity<PageObject, PageObjectEntity> {}