// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using ActivityPub.Types.AS;
using ActivityPub.Types.Conversion.Converters;
using ActivityPub.Types.Internal;
using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
///     Implements a "type graph", which is a form of emulated multiple inheritance.
///     This adapts the ActivityStreams dynamic type model to the C# static type model.
/// </summary>
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

    /// <summary>
    ///     JSON-LD context that describes this object.
    ///     This returns a live instance that will be automatically updated whenever the graph is extended.
    /// </summary>
    public IJsonLDContext LDContext => _ldContext;
    private readonly JsonLDContext _ldContext;

    /// <summary>
    ///     Properties that exist in the JSON, but did not map to any properties.
    /// </summary>
    [JsonInclude]
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? UnmappedProperties { internal get; set; }

    /// <summary>
    ///     Checks if the object contains a particular entity type.
    /// </summary>
    public bool IsEntity<TEntity>()
        where TEntity : ASEntity
        => _entityCache.ContainsKey(typeof(TEntity));

    /// <summary>
    ///     Checks if the object contains a particular type entity.
    ///     If so, then the instance of that type is extracted and returned.
    /// </summary>
    /// <seealso cref="IsEntity{TEntity}()" />
    /// <seealso cref="AsEntity{TEntity}" />
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
    ///     Gets an entity representing the object as entity type <code>TEntity</code>.
    /// </summary>
    /// <remarks>
    ///     This function will not extend the object to include a new type.
    ///     To safely convert to an instance that *might* be present, use <see cref="IsEntity{TEntity}(out TEntity)"/>.
    /// </remarks>
    /// <seealso cref="IsEntity{TEntity}(out TEntity)" />
    /// <exception cref="InvalidCastException">If the object is not of type <code>TEntity</code></exception>
    public TEntity AsEntity<TEntity>()
        where TEntity : ASEntity
    {
        var type = typeof(TEntity);
        if (!_entityCache.TryGetValue(type, out var instance))
            throw new InvalidCastException($"Can't represent the graph as entity {typeof(TEntity)}");
        return (TEntity)instance;
    }

    /// <summary>
    ///     Checks if the graph contains a particular model type.
    /// </summary>
    /// <seealso cref="IsModel{TModel}(out TModel)" />
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
    ///     Checks if the graph contains a particular model type.
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
    ///     Gets an object representing the graph as model type <code>TModel</code>.
    /// </summary>
    /// <remarks>
    ///     This function will not extend the object to include a new type.
    ///     To safely convert to an instance that *might* be present, use <see cref="IsModel{TModel}(out TModel)"/>.
    /// </remarks>
    /// <seealso cref="IsModel{TModel}()"/>
    /// <seealso cref="IsModel{TModel}(out TModel)" />
    /// <exception cref="InvalidCastException">If the graph cannot be represented by the type</exception>
    public TObject AsModel<TObject>()
        where TObject : ASType, IASModel<TObject>
    {
        if (IsModel<TObject>(out var instance))
            return instance;

        throw new InvalidCastException($"Can't represent the graph as type {typeof(TObject)}");
    }

    /// <summary>
    ///     Projects the type graph into a specific type of entity.
    ///     If <code>extendGraph</code> is <see langword="true"/>, then a new instance is constructed and linked to the graph.
    ///     Otherwise, an existing instance is returned.
    /// </summary>
    /// <remarks>
    ///     This function cannot accomodate entities with required members.
    ///     Instead, <see cref="Extend{TEntity}(TEntity)"/> must be used.
    /// </remarks>
    /// <exception cref="InvalidOperationException">If <code>extendGraph</code> is <see langword="true"/> and the entity type already exists in the graph</exception>
    /// <exception cref="InvalidOperationException">If <code>extendGraph</code> is <see langword="true"/> and the entity requires another entity that is missing from the graph</exception>
    /// <exception cref="InvalidCastException">If <code>extendGraph</code> is <see langword="false"/> and the object is not of type <code>TEntity</code></exception>
    /// <seealso cref="Extend{TEntity}()"/>
    /// <seealso cref="AsEntity{TEntity}"/>
    public TEntity ProjectTo<TEntity>(bool extendGraph)
        where TEntity : ASEntity, new() 
        => extendGraph
            ? Extend<TEntity>()
            : AsEntity<TEntity>();

    /// <summary>
    ///     Extends the TypeGraph to include a new entity.
    ///     The entity must have a parameterless constructor.
    ///     Throws an exception if the entity would be a duplicate or have unmet dependencies.
    /// </summary>
    /// <remarks>
    ///     This function cannot accomodate entities with required members.
    ///     Instead, <see cref="Extend{TEntity}(TEntity)"/> must be used.
    /// </remarks>
    /// <exception cref="InvalidOperationException">If the entity type already exists in the graph</exception>
    /// <exception cref="InvalidOperationException">If the entity requires another entity that is missing from the graph</exception>
    /// <see cref="Extend{TEntity}(TEntity)"/>
    /// <seealso cref="AddEntity"/>
    /// <seealso cref="TryAddEntity"/>
    public TEntity Extend<TEntity>()
        where TEntity : ASEntity, new()
        => Extend(new TEntity());
    
    /// <summary>
    ///     Extends the TypeGraph to include a new entity.
    ///     Throws an exception if the entity would be a duplicate or have unmet dependencies.
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">If the entity type already exists in the graph</exception>
    /// <exception cref="InvalidOperationException">If the entity requires another entity that is missing from the graph</exception>
    /// <see cref="Extend{TEntity}()"/>
    /// <seealso cref="AddEntity"/>
    /// <seealso cref="TryAddEntity"/>
    public TEntity Extend<TEntity>(TEntity entity)
        where TEntity : ASEntity
    {
        if (IsEntity<TEntity>())
            throw new InvalidOperationException($"Cannot extend the graph with entity {typeof(TEntity)}: that type already exists in the graph");
        
        // Check dependencies - this is a workaround to avoid the risk case described in TryAdd().
        if (entity.BaseTypeName != null && !AllASTypes.Contains(entity.BaseTypeName))
            throw new InvalidOperationException($"Cannot extend the graph with entity {typeof(TEntity)}: missing base type {entity.BaseTypeName}");
        
        AddEntity(entity);
        return entity;
    }

    /// <summary>
    ///     Like <see cref="TryAddEntity"/>, but throws if a conflicting entity already exists in the map.
    /// </summary>
    /// <exception cref="InvalidOperationException">If an object of this type already exists in the graph</exception>
    internal void AddEntity(ASEntity instance)
    {
        if (!TryAddEntity(instance))
            throw new InvalidOperationException($"Can't add {instance.GetType()} to graph - it already exists in the TypeMap");
    }

    /// <summary>
    ///     Adds a new typed instance to the graph.
    ///     Metadata such as AS types and JSON-LD context is automatically updated.
    ///     User code should not call this method; use <see cref="Extend{TEntity}()"/> instead.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method is internal as it has strict semantics regarding its use.
    ///         This is not a technical limitation, but rather an intentional choice to prevent the construction of invalid objects.
    ///         It is possible to induce confusing runtime crashes if a required entity is skipped.
    ///     </para>
    ///     <para>
    ///         For example, imagine that there is an empty TypeMap called "map".
    ///         The following calls are made:
    ///         <ul>
    ///             <li><code>map.AddEntity(new ASTypeEntity())</code></li>
    ///             <li><code>map.AddEntity(new ASActivityEntity())</code></li>
    ///             <li><code>map.AsModel&lt;ASActivity&gt;()</code></li>
    ///         </ul>
    ///         The last call will throw an <see cref="InvalidCastException"/> stating that the graph cannot be represented as <see cref="ASObjectEntity"/>.
    ///         This can be extremely hard to diagnose because <see cref="ASObjectEntity"/> does not even appear in the visible code.
    ///     </para>
    ///     <para>
    ///         The <see cref="Extend{TEntity}()"/> function, however, avoids this with an included dependency check.
    ///         The second call to <see cref="AddEntity"/> will fail with a descriptive error message indicating that <code>Object</code> is missing from the graph.
    ///         While still somewhat confusing, this method will additionally <b>fail fast</b> before the graph can even enter an invalid state.
    ///         This ensures that errors are caught quickly and before they can spread to corrupt the application state.
    ///     </para>
    /// </remarks>
    /// <returns>true if the type was added, false if it was already in the type map</returns>
    /// <seealso cref="Extend{TEntity}()"/>
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