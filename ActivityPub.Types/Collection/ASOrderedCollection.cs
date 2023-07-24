// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;
using static ActivityPub.Types.Collection.CollectionTypes;

namespace ActivityPub.Types.Collection;

/// <summary>
/// Collection where items have a strict ordering.
/// May be paged or unpaged.
/// </summary>
/// <typeparam name="T"></typeparam>
[ASTypeKey(OrderedCollectionType)]
public class ASOrderedCollection<T> : ASCollection<T>
    where T : ASObject
{
    [JsonConstructor]
    public ASOrderedCollection() : this(OrderedCollectionType) {}

    protected ASOrderedCollection(string type) : base(type) {}

    [JsonPropertyName("orderedItems")]
    public override LinkableList<T>? Items { get; set; }

    public static implicit operator ASOrderedCollection<T>(List<T> collection) => new() { Items = new(collection) };
    public static implicit operator ASOrderedCollection<T>(T value) => new() { Items = new() { value } };
}