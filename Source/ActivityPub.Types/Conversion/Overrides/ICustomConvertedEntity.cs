// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Nodes;

namespace ActivityPub.Types.Conversion.Overrides;

/// <summary>
///     An entity that requires custom JSON conversion logic.
/// </summary>
public interface ICustomConvertedEntity<TEntity>
    where TEntity : ASEntity, ICustomConvertedEntity<TEntity>
{
    /// <summary>
    ///     Converts the entity from a JSON message.
    ///     Return null to bypass and trigger built-in default logic.
    /// </summary>
    public static virtual TEntity? ReadEntity(JsonElement jsonElement, DeserializationMetadata meta) => null;
    
    /// <summary>
    ///     Called after the entity is converted from JSON.
    /// </summary>
    public static virtual void PostReadEntity(JsonElement jsonElement, DeserializationMetadata meta, TEntity entity) {}
    
    /// <summary>
    ///     Converts the entity into a JSON message.
    ///     Return null to bypass and trigger built-in default logic.
    /// </summary>
    public static virtual JsonElement? WriteEntity(TEntity entity, SerializationMetadata meta) => null;
    
    /// <summary>
    ///     Called after the object is converted to JSON.
    /// </summary>
    public static virtual void PostWriteEntity(TEntity entity, SerializationMetadata meta, JsonElement entityJson, JsonObject outputJson) {}
}