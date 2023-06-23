/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;

namespace ActivityPub.Types;

/// <summary>
/// A subtype of Collection in which members of the logical collection are assumed to always be strictly ordered.
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-orderedcollection"/>
public class ASOrderedCollection : ASCollection
{
    public const string OrderedCollectionType = "OrderedCollection";

    [JsonConstructor]
    public ASOrderedCollection() : this(OrderedCollectionType) {}

    protected ASOrderedCollection(string type) : base(type) {}
}