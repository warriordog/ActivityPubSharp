// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Collection;

/// <summary>
/// Collection that is unpaged and not ordered.
/// </summary>
public class ASUnpagedCollection<T> : ASCollection<T>
    where T : ASObject
{
    [JsonConstructor]
    public ASUnpagedCollection() {}

    protected ASUnpagedCollection(string type) : base(type) {}

    /// <summary>
    /// Identifies the items contained in a collection.
    /// The items might be ordered or unordered. 
    /// </summary>
    /// <remarks>
    /// In ordered collection types, this will map to "orderedItems".
    /// Otherwise, it maps to "items".
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-items"/>
    public virtual LinkableList<T> Items { get; set; } = new();
}