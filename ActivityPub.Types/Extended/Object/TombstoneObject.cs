// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// A Tombstone represents a content object that has been deleted.
/// It can be used in Collections to signify that there used to be an object at this position, but it has been deleted.
/// </summary>
[APTypeAttribute(TombstoneType)]
public class TombstoneObject : ASObject
{
    public const string TombstoneType = "Tombstone";

    [JsonConstructor]
    public TombstoneObject() : this(TombstoneType) {}

    protected TombstoneObject(string type) : base(type) {}

    /// <summary>
    /// On a Tombstone object, the formerType property identifies the type of the object that was deleted.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-formerType"/>
    [JsonPropertyName("formerType")]
    public string? FormerType { get; set; }

    /// <summary>
    /// On a Tombstone object, the deleted property is a timestamp for when the object was deleted. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-deleted"/>
    [JsonPropertyName("deleted")]
    public DateTime? Deleted { get; set; }
}
