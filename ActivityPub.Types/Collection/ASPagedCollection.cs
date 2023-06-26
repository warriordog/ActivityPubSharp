// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Collection;

/// <summary>
/// TODO documentation
/// </summary>
public class ASPagedCollection<T> : ASCollection<T>
    where T : ASObject
{
    [JsonConstructor]
    public ASPagedCollection() {}

    protected ASPagedCollection(string type) : base(type) {}

    /// <summary>
    ///  In a paged Collection, indicates the page that contains the most recently updated member items. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-current"/>
    public Linkable<ASCollectionPage<T>>? Current { get; set; }

    /// <summary>
    /// In a paged Collection, indicates the furthest preceding page of items in the collection. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-first"/>
    public Linkable<ASCollectionPage<T>>? First { get; set; }

    /// <summary>
    /// In a paged Collection, indicates the furthest proceeding page of the collection.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-attachment"/>
    public Linkable<ASCollectionPage<T>>? Last { get; set; }
}