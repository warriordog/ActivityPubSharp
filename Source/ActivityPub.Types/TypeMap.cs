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
    private readonly Dictionary<Type, ASEntity> _allEntities = new();
    private readonly CompositeASType _asTypes = new();

    // Cache of non-entity classes that have been constructed from this type graph.
    private readonly Dictionary<Type, ASType> _typeCache = new();

    /// <summary>
    ///     Constructs a new, empty type graph initialized with the ActivityStreams context.
    /// </summary>
    public TypeMap() : this(JsonLDContext.CreateASContext()) {}

    /// <summary>
    ///     Constructs a TypeMap with a pre-populated JSON-LD context.
    /// </summary>
    /// <param name="ldContext"></param>
    internal TypeMap(JsonLDContext ldContext) => _ldContext = ldContext;

    /// <summary>
    ///     Live set of all unique ActivityStreams types represented by this graph.
    /// </summary>
    /// <seealso cref="AllEntities" />
    public IReadOnlySet<string> ASTypes => _asTypes.Types;

    /// <summary>
    ///     Live map of .NET types to loaded entities contained in this graph.
    ///     This may be a subset or superset of ASTypes.
    /// </summary>
    /// <seealso cref="ASTypes" />
    public IReadOnlyDictionary<Type, ASEntity> AllEntities => _allEntities;

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
    public bool IsEntity<T>()
        where T : ASEntity
        => _allEntities.ContainsKey(typeof(T));

    /// <summary>
    ///     Checks if the object contains a particular type entity.
    ///     If so, then the instance of that type is extracted and returned.
    /// </summary>
    /// <seealso cref="IsEntity{T}()" />
    /// <seealso cref="AsEntity{T}" />
    public bool IsEntity<T>([NotNullWhen(true)] out T? instance)
        where T : ASEntity
    {
        if (_allEntities.TryGetValue(typeof(T), out var instanceT))
        {
            instance = (T)instanceT;
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
    public T AsEntity<T>()
        where T : ASEntity
    {
        var type = typeof(T);
        if (!_allEntities.TryGetValue(type, out var instance))
            throw new InvalidCastException($"Can't represent the graph as entity {typeof(T)}");
        return (T)instance;
    }

    /// <summary>
    ///     Gets an entity representing the graph as type T.
    ///     If the graph doesn't already include T, then it will be expanded with a new instance.
    /// </summary>
    public T ToEntity<T>()
        where T : ASEntity, new()
    {
        if (!IsEntity<T>(out var entity))
        {
            entity = new();
            Add(entity);
        }

        return entity;
    }

    /// <summary>
    ///     Checks if the graph contains a particular type.
    /// </summary>
    public bool IsType<TObject>()
        where TObject : ASType, IASModel<TObject>
    {
        var type = typeof(TObject);

        // Already cached -> yes
        if (_typeCache.ContainsKey(type))
            return true;

        // Can create it (entity is in the graph) -> yes
        if (_allEntities.ContainsKey(TObject.EntityType))
            return true;

        // Otherwise -> no
        return false;
    }

    /// <summary>
    ///     Checks if the graph contains a particular type.
    ///     If so, then the instance of that type is extracted and returned.
    /// </summary>
    /// <seealso cref="IsType{T}()" />
    /// <seealso cref="AsType{T}" />
    public bool IsType<TModel>([NotNullWhen(true)] out TModel? instance)
        where TModel : ASType, IASModel<TModel>
    {
        var type = typeof(TModel);

        // Already cached -> yes
        if (_typeCache.TryGetValue(type, out var instanceBase))
        {
            instance = (TModel)instanceBase;
            return true;
        }

        // Can create it (entity is in the graph) -> yes
        if (_allEntities.ContainsKey(TModel.EntityType))
        {
            instance = TModel.FromGraph(this);
            _typeCache[type] = instance;
            return true;
        }

        // Otherwise -> no
        instance = null;
        return false;
    }

    /// <summary>
    ///     Gets an object representing the graph as type T.
    /// </summary>
    /// <remarks>
    ///     This function will not extend the object to include a new type.
    ///     To safely convert to an instance that *might* be present, use Is().
    /// </remarks>
    /// <seealso cref="IsType{T}(out T?)" />
    /// <throws cref="InvalidCastException">If the graph cannot be represented by the type</throws>
    public TObject AsType<TObject>()
        where TObject : ASType, IASModel<TObject>
    {
        if (IsType<TObject>(out var instance))
            return instance;

        throw new InvalidCastException($"Can't represent the graph as type {typeof(TObject)}");
    }

    /// <summary>
    ///     Like <see cref="TryAdd"/>, but throws if a conflicting entity already exists in the map.
    /// </summary>
    /// <throws cref="InvalidOperationException">If an object of this type already exists in the graph</throws>
    internal void Add(ASEntity instance)
    {
        if (!TryAdd(instance))
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
    internal bool TryAdd(ASEntity entity)
    {
        var type = entity.GetType();
        if (_allEntities.ContainsKey(type))
            return false;

        // Map the instance
        _allEntities[type] = entity;

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