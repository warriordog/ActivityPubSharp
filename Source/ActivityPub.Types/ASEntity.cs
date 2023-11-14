// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.AS;

namespace ActivityPub.Types;

/// <summary>
///     Base type for AS entity classes.
///     Entities are singletons that contain data for a certain type within an object graph.
/// </summary>
public abstract class ASEntity
{
    internal ASEntity() {}

    /// <summary>
    ///     AS type name of this entity.
    ///     For the full list of names in the object graph, use <see cref="TypeMap.ASTypes" />.
    /// </summary>
    [JsonIgnore]
    public virtual string? ASTypeName => null;

    /// <summary>
    ///     AS type name of this entity's "base type".
    ///     When this entity is added to the graph, the target type will be removed if present.
    ///     This allows one entity type to "extend" another.
    ///     This should be set to false for extension types.
    /// </summary>
    /// <remarks>
    ///     This has no effect unless <see cref="ASTypeName"/> is non-null.
    /// </remarks>
    [JsonIgnore]
    public virtual string? BaseTypeName => null;

    /// <summary>
    ///     True if the object's current state can only be represented by the object form.
    ///     Objects that extend <see cref="ASLink"/> MUST return true if any properties are populated, other than HRef!
    /// </summary>
    [JsonIgnore]
    public virtual bool RequiresObjectForm => true;

    [JsonInclude]
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? UnmappedProperties { internal get; set; }
}

public abstract class ASEntity<TModel, TEntity> : ASEntity
    where TModel : IASModel<TModel, TEntity>
    where TEntity : ASEntity<TModel, TEntity>
{
    public sealed override string? ASTypeName => TModel.ASTypeName;
    public sealed override string? BaseTypeName => TModel.BaseTypeName;
}