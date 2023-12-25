// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     A Tombstone represents a content object that has been deleted.
///     It can be used in Collections to signify that there used to be an object at this position, but it has been deleted.
/// </summary>
public class TombstoneObject : ASObject, IASModel<TombstoneObject, TombstoneObjectEntity, ASObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Tombstone" types.
    /// </summary>
    [PublicAPI]
    public const string TombstoneType = "Tombstone";
    static string IASModel<TombstoneObject>.ASTypeName => TombstoneType;

    /// <inheritdoc />
    public TombstoneObject() => Entity = TypeMap.Extend<TombstoneObject, TombstoneObjectEntity>();

    /// <inheritdoc />
    public TombstoneObject(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<TombstoneObject, TombstoneObjectEntity>(isExtending);

    /// <inheritdoc />
    public TombstoneObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public TombstoneObject(TypeMap typeMap, TombstoneObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<TombstoneObject, TombstoneObjectEntity>();

    static TombstoneObject IASModel<TombstoneObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private TombstoneObjectEntity Entity { get; }

    /// <summary>
    ///     On a Tombstone object, the formerType property identifies the type of the object that was deleted.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-formerType" />
    public string? FormerType
    {
        get => Entity.FormerType;
        set => Entity.FormerType = value;
    }

    /// <summary>
    ///     On a Tombstone object, the deleted property is a timestamp for when the object was deleted.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-deleted" />
    public DateTime? Deleted
    {
        get => Entity.Deleted;
        set => Entity.Deleted = value;
    }
}

/// <inheritdoc cref="TombstoneObject" />
public sealed class TombstoneObjectEntity : ASEntity<TombstoneObject, TombstoneObjectEntity>
{
    /// <inheritdoc cref="TombstoneObject.FormerType" />
    [JsonPropertyName("formerType")]
    public string? FormerType { get; set; }

    /// <inheritdoc cref="TombstoneObject.Deleted" />
    [JsonPropertyName("deleted")]
    public DateTime? Deleted { get; set; }
}