// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// A Tombstone represents a content object that has been deleted.
/// It can be used in Collections to signify that there used to be an object at this position, but it has been deleted.
/// </summary>
public class TombstoneObject : ASObject
{
    private TombstoneObjectEntity Entity { get; }


    public TombstoneObject() => Entity = new TombstoneObjectEntity { TypeMap = TypeMap };
    public TombstoneObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<TombstoneObjectEntity>();

    /// <summary>
    /// On a Tombstone object, the formerType property identifies the type of the object that was deleted.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-formerType"/>
    public string? FormerType
    {
        get => Entity.FormerType;
        set => Entity.FormerType = value;
    }

    /// <summary>
    /// On a Tombstone object, the deleted property is a timestamp for when the object was deleted. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-deleted"/>
    public DateTime? Deleted
    {
        get => Entity.Deleted;
        set => Entity.Deleted = value;
    }
}

/// <inheritdoc cref="TombstoneObject"/>
[ASTypeKey(TombstoneType)]
[ImpliesOtherEntity(typeof(ASObjectEntity))]
public sealed class TombstoneObjectEntity : ASBase<TombstoneObject>
{
    public const string TombstoneType = "Tombstone";
    public override string ASTypeName => TombstoneType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>()
    {
        ASObjectEntity.ObjectType
    };

    /// <inheritdoc cref="TombstoneObject.FormerType"/>
    [JsonPropertyName("formerType")]
    public string? FormerType { get; set; }

    /// <inheritdoc cref="TombstoneObject.Deleted"/>
    [JsonPropertyName("deleted")]
    public DateTime? Deleted { get; set; }
}