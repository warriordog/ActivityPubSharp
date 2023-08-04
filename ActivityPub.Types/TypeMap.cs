// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ActivityPub.Types.Conversion.Converters;
using ActivityPub.Types.Conversion.Overrides;

namespace ActivityPub.Types;

[JsonConverter(typeof(TypeMapConverter))]
public class TypeMap
{
    private readonly Dictionary<Type, ASBase> _allEntities = new();

    private readonly HashSet<string> _asTypes = new();

    // All AS types that are replaced by at least one entity in the type graph
    private readonly HashSet<string> _replacedASTypes = new();

    // Cache of non-entity classes
    private readonly Dictionary<Type, ASType> _typeCache = new();

    // Map non-entities to entity that can construct it
    private readonly Dictionary<Type, ASBase> _typeEntityMap = new();

    /// <summary>
    ///     Live set of all unique ActivityStreams types represented by this graph.
    /// </summary>
    /// <seealso cref="AllEntities" />
    public IReadOnlySet<string> ASTypes => _asTypes;

    /// <summary>
    ///     Live map of .NET types to loaded entities contained in this graph.
    ///     This may be a subset or superset of ASTypes.
    /// </summary>
    /// <seealso cref="ASTypes" />
    public IReadOnlyDictionary<Type, ASBase> AllEntities => _allEntities;

    /// <summary>
    ///     Reference to the single entity which implements <see cref="IJsonValueSerialized{TThis}" />
    ///     Will be null if none is present.
    /// </summary>
    internal ASBase? ValueSerializer { get; private set; }

    /// <summary>
    ///     Checks if the object contains a particular type entity.
    /// </summary>
    public bool IsEntity<T>()
        where T : ASBase
        => _allEntities.ContainsKey(typeof(T));

    /// <summary>
    ///     Checks if the object contains a particular type entity.
    ///     If so, then the instance of that type is extracted and returned.
    /// </summary>
    /// <seealso cref="IsEntity{T}()" />
    /// <seealso cref="AsEntity{T}" />
    public bool IsEntity<T>([NotNullWhen(true)] out T? instance)
        where T : ASBase
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
        where T : ASBase
    {
        var type = typeof(T);
        if (!_allEntities.TryGetValue(type, out var instance))
            throw new InvalidCastException($"Can't represent the graph as entity {typeof(T)}");
        return (T)instance;
    }

    /// <summary>
    ///     Checks if the graph contains a particular type.
    /// </summary>
    public bool IsType<T>()
        where T : ASType
    {
        var type = typeof(T);

        // Already cached -> yes
        if (_typeCache.ContainsKey(type))
            return true;

        // Can be converted -> yes
        if (_typeEntityMap.ContainsKey(type))
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
    public bool IsType<T>([NotNullWhen(true)] out T? instance)
        where T : ASType
    {
        var type = typeof(T);

        // Already cached -> yes
        if (_typeCache.TryGetValue(type, out var instanceBase))
        {
            instance = (T)instanceBase;
            return true;
        }

        // Can create it -> yes
        if (_typeEntityMap.TryGetValue(type, out var entity))
        {
            instance = (T)entity.CreateTypeInstanceBase(this);
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
    public T AsType<T>()
        where T : ASType
    {
        if (IsType<T>(out var instance))
            return instance;

        throw new InvalidCastException($"Can't represent the graph as type {typeof(T)}");
    }

    /// <summary>
    ///     Adds a new typed instance to the object.
    /// </summary>
    /// <remarks>
    ///     This method is internal, as it should only be called by <see cref="ASBase" /> constructor.
    ///     User code should instead add a new type by passing an existing TypeMap into the constructor.
    ///     This is not a technical limitation, but rather an intentional choice to prevent the construction of invalid objects.
    /// </remarks>
    /// <throws cref="InvalidOperationException">If an object of this type already exists in the graph</throws>
    internal void Add(ASBase instance)
    {
        var type = instance.GetType();

        if (_allEntities.ContainsKey(type))
            throw new InvalidOperationException($"Can't add {type} to graph - it already exists in the TypeMap");

        // Map value serializer.
        // This is first since it has an additional check.
        if (instance.IsValueSerialized)
        {
            if (ValueSerializer != null)
                throw new InvalidOperationException($"Can't add {type} to graph - it conflicts with another entity that also implements IJsonValueSerialized");

            ValueSerializer = instance;
        }

        // Map the instance
        _allEntities[type] = instance;
        _typeEntityMap[instance.NonEntityType] = instance;

        // Map the AS type
        if (instance.ASTypeName != null)
        {
            // Update the replaced list
            if (instance.ReplacesASTypes != null)
                foreach (var replacedType in instance.ReplacesASTypes)
                    _replacedASTypes.Add(replacedType);

            // Add it to the type list and synchronize
            _asTypes.Add(instance.ASTypeName);
            _asTypes.RemoveWhere(
                asType
                    => _replacedASTypes.Contains(asType)
            );
        }
    }
}