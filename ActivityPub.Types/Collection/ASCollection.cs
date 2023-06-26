/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using ActivityPub.Types.Json;
using static ActivityPub.Types.Collection.CollectionTypes;

namespace ActivityPub.Types.Collection;


/// <summary>
/// A Collection is a subtype of Object that represents ordered or unordered sets of Object or Link instances.
/// Data access varies by subtype - see <see cref="ASPagedCollection{T}"/> and <see cref="ASUnpagedCollection{T}"/> for more info.
/// </summary>
/// <remarks>
/// Refer to the <a href="https://www.w3.org/TR/activitystreams-core/#collection">Activity Streams 2.0 Core specification</a> for a complete description of the Collection type.
/// TODO maybe add a non-generic base between this and ASObject for better DX 
/// </remarks>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-collection"/>
[ASTypeKey(CollectionType)]
[ASTypeKey(OrderedCollectionType)]
// ReSharper disable once UnusedTypeParameter (needed to provide a common root for generic subclasses)
public abstract class ASCollection<T> : ASObject
    where T : ASObject
{
    protected ASCollection() : this(CollectionType) {}
    protected ASCollection(string type) : base(type) {}

    /// <summary>
    /// A non-negative integer specifying the total number of objects contained by the logical view of the collection.
    /// This number might not reflect the actual number of items serialized within the Collection object instance. 
    /// </summary>
    public int TotalItems { get; set; }
}