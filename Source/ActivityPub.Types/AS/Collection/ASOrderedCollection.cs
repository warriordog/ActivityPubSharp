// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Collection;

/// <summary>
///     A Collection is a subtype of Object that represents ordered sets of Object or Link instances.
///     May be paged or unpaged.
/// </summary>
/// <remarks>
///     Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the Collection type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-collection" />
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-orderedcollection" />
public class ASOrderedCollection : ASObject, IASModel<ASOrderedCollection, ASOrderedCollectionEntity, ASObject>, IEnumerable<Linkable<ASObject>>
{
    /// <summary>
    ///     ActivityStreams type name for "OrderedCollection" types.
    /// </summary>
    [PublicAPI]
    public const string OrderedCollectionType = "OrderedCollection";
    static string IASModel<ASOrderedCollection>.ASTypeName => OrderedCollectionType;

    /// <inheritdoc />
    public ASOrderedCollection() => Entity = TypeMap.Extend<ASOrderedCollection, ASOrderedCollectionEntity>();

    /// <inheritdoc />
    public ASOrderedCollection(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<ASOrderedCollection, ASOrderedCollectionEntity>(isExtending);

    /// <inheritdoc />
    public ASOrderedCollection(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public ASOrderedCollection(TypeMap typeMap, ASOrderedCollectionEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ASOrderedCollection, ASOrderedCollectionEntity>();

    static ASOrderedCollection IASModel<ASOrderedCollection>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ASOrderedCollectionEntity Entity { get; }


    /// <summary>
    ///     In a paged Collection, indicates the page that contains the most recently updated member items.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-current" />
    public Linkable<ASOrderedCollectionPage>? Current
    {
        get => Entity.Current;
        set => Entity.Current = value;
    }

    /// <summary>
    ///     In a paged Collection, indicates the furthest preceding page of items in the collection.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-first" />
    public Linkable<ASOrderedCollectionPage>? First
    {
        get => Entity.First;
        set => Entity.First = value;
    }

    /// <summary>
    ///     In a paged Collection, indicates the furthest proceeding page of the collection.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-last" />
    public Linkable<ASOrderedCollectionPage>? Last
    {
        get => Entity.Last;
        set => Entity.Last = value;
    }

    /// <summary>
    ///     A non-negative integer specifying the total number of objects contained by the logical view of the collection.
    ///     This number might not reflect the actual number of items serialized within the Collection object instance.
    /// </summary>
    /// <remarks>
    ///     If not explicitly set, then this will default to the number of elements in <see cref="Items" />.
    ///     Setting this to any value will disable this logic.
    ///     If neither is set, returns zero.
    ///     This cannot be set to less than zero.
    /// </remarks>
    public int TotalItems
    {
        get => Entity.TotalItems;
        set => Entity.TotalItems = value;
    }

    /// <summary>
    ///     Identifies the items contained in a collection.
    ///     The items might be ordered or unordered.
    /// </summary>
    /// <remarks>
    ///     In ordered collection types, this will map to "orderedItems".
    ///     Otherwise, it maps to "items".
    ///     In a paged collection, this MAY be null
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-items" />
    public LinkableList<ASObject>? Items
    {
        get => Entity.Items;
        set => Entity.Items = value;
    }

    /// <summary>
    ///     True if this is a paged collection, false otherwise.
    /// </summary>
    [MemberNotNullWhen(true, nameof(FirstPopulated))]
    public bool IsPaged => Current != null || First != null || Last != null;

    /// <summary>
    ///     In a paged Collection, gets the first populated entity.
    /// </summary>
    public Linkable<ASOrderedCollectionPage>? FirstPopulated => Current ?? First ?? Last;

    /// <summary>
    ///     True if this collection instance contains items, false otherwise.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Items))]
    public bool HasItems => Items?.Any() == true;

    /// <summary>
    ///     Converts a list of items into an ordered collection.
    /// </summary>
    public static implicit operator ASOrderedCollection(List<ASObject> collection) => new()
    {
        Items = new LinkableList<ASObject>(collection)
    };

    /// <inheritdoc />
    public IEnumerator<Linkable<ASObject>> GetEnumerator() => Items?.GetEnumerator() ?? Enumerable.Empty<Linkable<ASObject>>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <inheritdoc cref="ASOrderedCollection" />
public sealed class ASOrderedCollectionEntity : ASEntity<ASOrderedCollection, ASOrderedCollectionEntity>
{
    /// <inheritdoc cref="ASOrderedCollection.Current" />
    [JsonPropertyName("current")]
    public Linkable<ASOrderedCollectionPage>? Current { get; set; }

    /// <inheritdoc cref="ASOrderedCollection.First" />
    [JsonPropertyName("first")]
    public Linkable<ASOrderedCollectionPage>? First { get; set; }

    /// <inheritdoc cref="ASOrderedCollection.Last" />
    [JsonPropertyName("last")]
    public Linkable<ASOrderedCollectionPage>? Last { get; set; }

    /// <inheritdoc cref="ASOrderedCollection.TotalItems" />
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [Range(0, int.MaxValue)]
    [JsonPropertyName("totalItems")]
    public int TotalItems
    {
        get => _totalItems ?? Items?.Count ?? 0;
        set => _totalItems = Math.Max(value, 0);
    }

    private int? _totalItems;

    /// <inheritdoc cref="ASOrderedCollection.Items" />
    [JsonPropertyName("orderedItems")]
    public LinkableList<ASObject>? Items { get; set; }
}