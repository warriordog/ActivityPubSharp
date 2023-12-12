// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using ActivityPub.Types.Conversion.Overrides;

namespace ActivityPub.Types.Internal.Pivots;

internal class CustomConvertedEntityPivot
{
    private readonly Dictionary<Type, bool> _knownCustomConverters = new();
    
    /// <summary>
    ///     Pivots to <see cref="ReadEntity{TEntity}"/> from a <see cref="Type"/> provided as first argument.
    /// </summary>
    private readonly Func<Type, JsonElement, DeserializationMetadata, ASEntity?> _readEntityPivot =
        typeof(CustomConvertedEntityPivot)
        .GetRequiredMethod(nameof(ReadEntity), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
        .CreateGenericPivotFunc<JsonElement, DeserializationMetadata, ASEntity?>();
    
    /// <summary>
    ///     Pivots to <see cref="PostReadEntity{TEntity}"/> from a <see cref="Type"/> provided as first argument.
    /// </summary>
    private readonly Action<Type, JsonElement, DeserializationMetadata, ASEntity> _postReadEntityPivot = 
        typeof(CustomConvertedEntityPivot)
            .GetRequiredMethod(nameof(PostReadEntity), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
            .CreateGenericPivotAction<JsonElement, DeserializationMetadata, ASEntity>();
    
    /// <summary>
    ///     Pivots to <see cref="WriteEntity{TEntity}"/> from a <see cref="Type"/> provided as first argument.
    /// </summary>
    private readonly Func<Type, ASEntity, SerializationMetadata, JsonElement?> _writeEntityPivot =
        typeof(CustomConvertedEntityPivot)
            .GetRequiredMethod(nameof(WriteEntity), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
            .CreateGenericPivotFunc<ASEntity, SerializationMetadata, JsonElement?>();
    
    /// <summary>
    ///     Pivots to <see cref="PostWriteEntity{TEntity}"/> from a <see cref="Type"/> provided as first argument.
    /// </summary>
    private readonly Action<Type, ASEntity, SerializationMetadata, JsonElement, JsonObject> _postWriteEntityPivot =
        typeof(CustomConvertedEntityPivot)
            .GetRequiredMethod(nameof(PostWriteEntity), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
            .CreateGenericPivotAction<ASEntity, SerializationMetadata, JsonElement, JsonObject>();


    public ASEntity? ReadEntity(Type entityType, JsonElement jsonElement, DeserializationMetadata meta)
    {
        if (IsCustomConverted(entityType))
            return _readEntityPivot(entityType, jsonElement, meta);

        return null;
    }

    public void PostReadEntity(Type entityType, JsonElement jsonElement, DeserializationMetadata meta, ASEntity entity)
    {
        if (IsCustomConverted(entityType))
            _postReadEntityPivot(entityType, jsonElement, meta, entity);
    }

    public JsonElement? WriteEntity(Type entityType, ASEntity entity, SerializationMetadata meta)
    {
        if (IsCustomConverted(entityType))
            return _writeEntityPivot(entityType, entity, meta);

        return null;
    }

    public void PostWriteEntity(Type entityType, ASEntity entity, SerializationMetadata meta, JsonElement entityJson, JsonObject outputJson)
    {
        if (IsCustomConverted(entityType))
            _postWriteEntityPivot(entityType, entity, meta, entityJson, outputJson);
    }

    /// <summary>
    ///     Checks if a provided entity type is an implementation of <see cref="ICustomConvertedEntity{TEntity}"/>.
    /// </summary>
    private bool IsCustomConverted(Type entityType)
    {
        if (!_knownCustomConverters.TryGetValue(entityType, out var isCustomConverted))
        {
            // This is all just a complicated way to avoid a crash in the case where entityType does *not* implement ICustomConvertedEntity<entityType>
            if (typeof(ICustomConvertedEntity<>).TryMakeGenericType(out var interfaceType, entityType))
                isCustomConverted = entityType.IsAssignableTo(interfaceType);
            else
                isCustomConverted = false;
            
            _knownCustomConverters.Add(entityType, isCustomConverted);
        }

        return isCustomConverted;
    }


    /// <summary>
    ///     Calls <see cref="ICustomConvertedEntity{TEntity}.ReadEntity"/> for a given type of TEntity.
    /// </summary>
    private static TEntity? ReadEntity<TEntity>(JsonElement jsonElement, DeserializationMetadata meta)
        where TEntity : ASEntity, ICustomConvertedEntity<TEntity>
        => TEntity.ReadEntity(jsonElement, meta);

    /// <summary>
    ///     Calls <see cref="ICustomConvertedEntity{TEntity}.PostReadEntity"/> for a given type of TEntity.
    ///     Parameter types are relaxed to simplify generic binding.
    /// </summary>
    private static void PostReadEntity<TEntity>(JsonElement jsonElement, DeserializationMetadata meta, ASEntity entity)
        where TEntity : ASEntity, ICustomConvertedEntity<TEntity>
        => TEntity.PostReadEntity(jsonElement, meta, (TEntity)entity);

    /// <summary>
    ///     Calls <see cref="ICustomConvertedEntity{TEntity}.WriteEntity"/> for a given type of TEntity.
    ///     Parameter types are relaxed to simplify generic binding.
    /// </summary>
    private static JsonElement? WriteEntity<TEntity>(ASEntity entity, SerializationMetadata meta)
        where TEntity : ASEntity, ICustomConvertedEntity<TEntity>
        => TEntity.WriteEntity((TEntity)entity, meta);

    /// <summary>
    ///     Calls <see cref="ICustomConvertedEntity{TEntity}.PostWriteEntity"/> for a given type of TEntity.
    ///     Parameter types are relaxed to simplify generic binding.
    /// </summary>
    private static void PostWriteEntity<TEntity>(ASEntity entity, SerializationMetadata meta, JsonElement entityJson, JsonObject outputJson)
        where TEntity : ASEntity, ICustomConvertedEntity<TEntity>
        => TEntity.PostWriteEntity((TEntity)entity, meta, entityJson, outputJson);
}