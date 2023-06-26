// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using static ActivityPub.Types.Collection.CollectionTypes;

namespace ActivityPub.Types.Collection;

/// <summary>
/// Collection that is both paged and ordered.
/// </summary>
public class ASOrderedPagedCollection<T> : ASPagedCollection<T>
    where T : ASObject
{
    [JsonConstructor]
    public ASOrderedPagedCollection() : this(OrderedCollectionType) {}

    protected ASOrderedPagedCollection(string type) : base(type) {}
}