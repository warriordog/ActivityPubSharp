// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.AS;
using ActivityPub.Types.Conversion.Converters;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Util;
using InternalUtils;

namespace ActivityPub.Types;

[JsonConverter(typeof(TypeMapConverter))]
public class TypeMap
{
    // Cache of AS type names that exist in this type graph.
    private readonly CompositeASType _asTypes = new();

    // Cache of model classes that have been constructed from this type graph.
    private readonly Dictionary<Type, ASType> _modelCache = new();
    
    // Cache of entity classes that have been added to this type graph.
    private readonly Dictionary<Type, ASEntity> _entityCache = new();

    /// <summary>
    ///     Constructs a new, empty type graph initialized with the ActivityStreams context.
    /// </summary>
    public TypeMap() : this(JsonLDContext.CreateASContext()) {}

    /// <summary>
    ///     Constructs a TypeMap with a pre-populated JSON-LD context.
    /// </summary>
    /// <param name="ldContext"></param>
    internal TypeMap(JsonLDContext ldContext) => _ldContext = ldContext;


    /// <inheritdoc cref="CompositeASType.Types"/>
    /// <seealso cref="AllASTypes"/>
    public IReadOnlySet<string> ASTypes => _asTypes.Types;

    /// <inheritdoc cref="CompositeASType.AllTypes"/>
    /// <seealso cref="ASTypes"/>
    public IReadOnlySet<string> AllASTypes => _asTypes.AllTypes;

    /// <summary>
    ///     Live map of .NET types to loaded entities contained in this graph.
    ///     This may be a subset or superset of ASTypes.
    /// </summary>
    /// <seealso cref="ASTypes" />
    public IReadOnlyDictionary<Type, ASEntity> AllEntities => _entityCache;

    public IJsonLDContext LDContext => _ldContext;
    private readonly JsonLDContext _ldContext;

    /// <summary>
    ///     Properties that exist in the JSON, but did not map to any properties.
    /// </summary>
    [JsonInclude]
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? UnmappedProperties { internal get; set; }

    /// <summary>
    ///     Checks if the object contains a particular type entity.
    /// </summary>
    public bool IsEntity<TEntity>()
        where TEntity : ASEntity
        => _entityCache.ContainsKey(typeof(TEntity));

    /// <summary>
    ///     Checks if the object contains a particular type entity.
    ///     If so, then the instance of that type is extracted and returned.
    /// </summary>
    /// <seealso cref="IsEntity{T}()" />
    /// <seealso cref="AsEntity{T}" />
    public bool IsEntity<TEntity>([NotNullWhen(true)] out TEntity? instance)
        where TEntity : ASEntity
    {
        if (_entityCache.TryGetValue(typeof(TEntity), out var instanceT))
        {
            instance = (TEntity)instanceT;
            return true;
        }

        instance = null;
        return false;
    }

    /// <summary>
    ///     Gets an entity representing the object as type T.
    /// </summary>
    /// <remarks>
    ///     This function will not extend the object to include a new type.
    ///     To safely convert to an instance that *might* be present, use Is().
    /// </remarks>
    /// <seealso cref="IsEntity{T}(out T?)" />
    /// <throws cref="InvalidCastException">If the object is not of type T</throws>
    public TEntity AsEntity<TEntity>()
        where TEntity : ASEntity
    {
        var type = typeof(TEntity);
        if (!_entityCache.TryGetValue(type, out var instance))
            throw new InvalidCastException($"Can't represent the graph as entity {typeof(TEntity)}");
        return (TEntity)instance;
    }

    /// <summary>
    ///     Checks if the graph contains a particular model.
    /// </summary>
    /// <seealso cref="IsModel{TModel}(out TModel?)" />
    /// <seealso cref="AsModel{TModel}()" />
    public bool IsModel<TModel>()
        where TModel : ASType, IASModel<TModel>
    {
        var type = typeof(TModel);

        // Already cached -> yes
        if (_modelCache.ContainsKey(type))
            return true;

        // Can create it (entity is in the graph) -> yes
        if (_entityCache.ContainsKey(TModel.EntityType))
            return true;

        // Otherwise -> no
        return false;
    }

    /// <summary>
    ///     Checks if the graph contains a particular model.
    ///     If so, then the instance of that model is extracted and returned.
    /// </summary>
    /// <seealso cref="IsModel{TModel}()"/>
    /// <seealso cref="AsModel{TModel}()" />
    public bool IsModel<TModel>([NotNullWhen(true)] out TModel? instance)
        where TModel : ASType, IASModel<TModel>
    {
        var type = typeof(TModel);

        // Already cached -> yes
        if (_modelCache.TryGetValue(type, out var instanceBase))
        {
            instance = (TModel)instanceBase;
            return true;
        }

        // Can create it (entity is in the graph) -> yes
        if (_entityCache.ContainsKey(TModel.EntityType))
        {
            instance = TModel.FromGraph(this);
            _modelCache[type] = instance;
            return true;
        }

        // Otherwise -> no
        instance = null;
        return false;
    }

    /// <summary>
    ///     Gets an object representing the graph as model type T.
    /// </summary>
    /// <remarks>
    ///     This function will not extend the object to include a new type.
    ///     To safely convert to an instance that *might* be present, use Is().
    /// </remarks>
    /// <seealso cref="IsModel{TModel}()"/>
    /// <seealso cref="IsModel{TModel}(out TModel?)" />
    /// <throws cref="InvalidCastException">If the graph cannot be represented by the type</throws>
    public TObject AsModel<TObject>()
        where TObject : ASType, IASModel<TObject>
    {
        if (IsModel<TObject>(out var instance))
            return instance;

        throw new InvalidCastException($"Can't represent the graph as type {typeof(TObject)}");
    }

    /// <summary>
    ///     Like <see cref="TryAddEntity"/>, but throws if a conflicting entity already exists in the map.
    /// </summary>
    /// <throws cref="InvalidOperationException">If an object of this type already exists in the graph</throws>
    internal void AddEntity(ASEntity instance)
    {
        if (!TryAddEntity(instance))
            throw new InvalidOperationException($"Can't add {instance.GetType()} to graph - it already exists in the TypeMap");
    }

    /// <summary>
    ///     Adds a new typed instance to the object.
    ///     Metadata such as AS types and JSON-LD context is automatically updated.
    /// </summary>
    /// <remarks>
    ///     This method is internal, as it should only be called by <see cref="ASEntity" /> constructor.
    ///     User code should instead add a new type by passing an existing TypeMap into the constructor.
    ///     This is not a technical limitation, but rather an intentional choice to prevent the construction of invalid objects.
    /// </remarks>
    /// <returns>true if the type was added, false if it was already in the type map</returns>
    internal bool TryAddEntity(ASEntity entity)
    {
        var type = entity.GetType();
        if (_entityCache.ContainsKey(type))
            return false;

        // Map the instance
        _entityCache[type] = entity;

        // Map the AS type
        if (entity.ASTypeName != null)
            _asTypes.Add(entity.ASTypeName, entity.BaseTypeName);

        // Populate and/or filter the list of unmapped properties
        NarrowUnmappedProperties(entity);

        // Register the JSON-LD context
        _ldContext.AddRange(entity.DefiningContext);
        
        return true;
    }


    // Inefficient implementation
    private void NarrowUnmappedProperties(ASEntity entity)
    {
        // Skip if the properties weren't populated
        if (entity.UnmappedProperties == null)
            return;

        // If this is the first entity, then just directly set the map
        if (UnmappedProperties == null)
            UnmappedProperties = entity.UnmappedProperties;

        // Otherwise, we merge.
        else
            UnmappedProperties.Keys
                .Except(entity.UnmappedProperties.Keys)
                .ToList()
                .ForEach(k => UnmappedProperties.Remove(k));
    }

    /// <summary>
    ///     Records an ActivityStreams type name as being present in the type graph, but not mapped to any known implementation class.
    ///     Duplicates will be ignored.
    /// </summary>
    internal void AddUnmappedType(string asTypeName) => _asTypes.Add(asTypeName);
}