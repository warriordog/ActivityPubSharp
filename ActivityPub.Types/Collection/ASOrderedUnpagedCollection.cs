// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Util;
using static ActivityPub.Types.Collection.CollectionTypes;

namespace ActivityPub.Types.Collection;

/// <summary>
/// Collection that is ordered but not paged.
/// </summary>
public class ASOrderedUnpagedCollection<T> : ASUnpagedCollection<T>
    where T : ASObject
{
    [JsonConstructor]
    public ASOrderedUnpagedCollection() : this(OrderedCollectionType) {}

    protected ASOrderedUnpagedCollection(string type) : base(type) {}
    
    [JsonPropertyName("orderedItems")]
    public override LinkableList<T> Items { get; set; } = new();
}