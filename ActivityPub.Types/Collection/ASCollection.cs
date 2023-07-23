// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;
using static ActivityPub.Types.Collection.CollectionTypes;

namespace ActivityPub.Types.Collection;

/// <summary>
/// A Collection is a subtype of Object that represents ordered or unordered sets of Object or Link instances.
/// May be paged or unpaged, and ordered or unordered. 
/// </summary>
/// <remarks>
/// Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the Collection type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-collection"/>
/// <seealso cref="ASOrderedCollection{T}"/>
[ASTypeKey(CollectionType)]
public class ASCollection<T> : ASObject
    where T : ASType
{
    [JsonConstructor]
    public ASCollection() : this(CollectionType) {}

    protected ASCollection(string type) : base(type) {}

    /// <summary>
    ///  In a paged Collection, indicates the page that contains the most recently updated member items. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-current"/>
    [JsonPropertyName("current")]
    public Linkable<ASCollectionPage<T>>? Current { get; set; }

    /// <summary>
    /// In a paged Collection, indicates the furthest preceding page of items in the collection. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-first"/>
    [JsonPropertyName("first")]
    public Linkable<ASCollectionPage<T>>? First { get; set; }

    /// <summary>
    /// In a paged Collection, indicates the furthest proceeding page of the collection.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-last"/>
    [JsonPropertyName("last")]
    public Linkable<ASCollectionPage<T>>? Last { get; set; }

    /// <summary>
    /// A non-negative integer specifying the total number of objects contained by the logical view of the collection.
    /// This number might not reflect the actual number of items serialized within the Collection object instance. 
    /// </summary>
    /// <remarks>
    /// If not explicitly set, then this will default to the number of elements in <see cref="Items"/>.
    /// Setting this to any value will disable this logic.
    /// If neither is set, returns zero.
    /// This cannot be set to less than zero.
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [Range(0, int.MaxValue)]
    [JsonPropertyName("totalItems")]
    public int TotalItems
    {
        get => _totalItems ?? Items?.Count ?? 0;
        set => _totalItems = Math.Max(value, 0);
    }

    private int? _totalItems;

    /// <summary>
    /// Identifies the items contained in a collection.
    /// The items might be ordered or unordered. 
    /// </summary>
    /// <remarks>
    /// In ordered collection types, this will map to "orderedItems".
    /// Otherwise, it maps to "items".
    /// In a paged collection, this MAY be null
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-items"/>
    [JsonPropertyName("items")]
    public virtual List<T>? Items { get; set; }

    /// <summary>
    /// True if this is a paged collection, false otherwise.
    /// </summary>
    [JsonIgnore]
    public bool IsPaged => Current != null || First != null || Last != null;

    /// <summary>
    /// True if this collection instance contains items, false otherwise.
    /// </summary>
    [JsonIgnore]
    [MemberNotNullWhen(true, nameof(Items))]
    public bool HasItems => Items?.Any() == true;


    public static implicit operator ASCollection<T>(List<T> collection) => new() { Items = new(collection) };
    public static implicit operator ASCollection<T>(T value) => new() { Items = new() { value } };
}