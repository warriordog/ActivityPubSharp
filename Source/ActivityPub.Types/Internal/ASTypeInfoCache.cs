// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ActivityPub.Types.AS;
using ActivityPub.Types.Conversion.Overrides;

namespace ActivityPub.Types.Internal;

/// <summary>
///     Extracts and stores metadata for ActivityStreams types within the application.
/// </summary>
internal interface IASTypeInfoCache
{
    /// <summary>
    ///     Finds the .NET type(s) that implement a set of AS types.
    ///     Implied types are automatically included.
    ///     Unknown types are safely ignored.
    /// </summary>
    /// <param name="asTypes">Types to map. Case-sensitive.</param>
    /// <param name="mappedEntities">All known entities that represent the types.</param>
    /// <param name="unmappedTypes">Any AS types that did not map to an entity.</param>
    /// <returns>Set of all located types</returns>
    internal void MapASTypesToEntities(IEnumerable<string> asTypes, out HashSet<Type> mappedEntities, out HashSet<string> unmappedTypes);

    /// <summary>
    ///     Gets the .NET type that implements a specified AS type.
    ///     Returns true on success or false on failure, following the TryGet pattern.
    /// </summary>
    bool TryGetModelType(string asType, [NotNullWhen(true)] out Type? modelType);
    
    /// <summary>
    ///     All known types that implement <see cref="IAnonymousEntity"/>.
    /// </summary>
    internal IEnumerable<Type> AnonymousEntityTypes { get; }

    /// <summary>
    ///     All known types that implement <see cref="INamelessEntity"/>.
    /// </summary>
    internal IEnumerable<Type> NamelessEntityTypes { get; }
    
    /// <summary>
    ///     Find and load all ActivityStreams types in a particular assembly.
    /// </summary>
    /// <param name="assembly">Assembly to load</param>
    void RegisterAssembly(Assembly assembly);

    /// <summary>
    ///     Find and load all ActivityStreams types in all loaded assemblies.
    /// </summary>
    void RegisterAllAssemblies();
}

internal class ASTypeInfoCache : IASTypeInfoCache
{
    private readonly HashSet<Assembly> _registeredAssemblies = new();
    
    private readonly Dictionary<Type, ModelMeta> _typeMetaMap = new();
    private readonly Dictionary<string, ModelMeta> _nameMetaMap = new();

    public IEnumerable<Type> AnonymousEntityTypes => _anonymousEntityTypes;
    private readonly HashSet<Type> _anonymousEntityTypes = new();

    public IEnumerable<Type> NamelessEntityTypes => _namelessEntityTypes;
    private readonly HashSet<Type> _namelessEntityTypes = new();

    /// <summary>
    ///     Calls <see cref="CreateModelMetaFor{TModel}"/> with a specified value for T.
    /// </summary>
    private readonly Func<Type, ModelMeta> _createTypeMetadataFor;

    internal ASTypeInfoCache() =>
        // I really hate doing this :sob:
        _createTypeMetadataFor = typeof(ASTypeInfoCache)
            .GetRequiredMethod(nameof(CreateModelMetaFor), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
            .CreateGenericPivotFunc<ModelMeta>(this);

    public void MapASTypesToEntities(IEnumerable<string> asTypes, out HashSet<Type> mappedEntities, out HashSet<string> unmappedTypes)
    {
        mappedEntities = new HashSet<Type>();
        unmappedTypes = new HashSet<string>();

        foreach (var asType in asTypes)
        {
            // Map AS Type to .NET Type.
            if (_nameMetaMap.TryGetValue(asType, out var meta))
                mappedEntities.AddRange(meta.EntityTypeChain);

            // Record unknown types too
            else
                unmappedTypes.Add(asType);
        }
    }

    public bool TryGetModelType(string asType, [NotNullWhen(true)] out Type? modelType)
    {
        if (_nameMetaMap.TryGetValue(asType, out var meta))
        {
            modelType = meta.ModelType;
            return true;
        }

        modelType = null;
        return false;
    }

    public void RegisterAllAssemblies()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            RegisterAssembly(assembly);
    }

    public void RegisterAssembly(Assembly assembly)
    {
        // Make sure we only check each assembly once.
        // Its an extremely heavy operation.
        if (!_registeredAssemblies.Add(assembly))
            return;

        // This is a new assembly, so we need to check every type
        foreach (var type in assembly.GetTypes())
            RegisterType(type);
    }

    private void RegisterType(Type type)
    {
        // Skip if we've already check it
        if (_typeMetaMap.ContainsKey(type))
            return;

        // Skip if it's not an AS type
        if (!type.IsAssignableTo(typeof(ASType)))
            return;

        // Skip if it's not a model
        if (!typeof(IASModel<>).TryMakeGenericType(out var modelType, type))
            return;
        if (!type.IsAssignableTo(modelType))
            return;

        // Make sure that base types are populated first
        if (type.BaseType != null)
            RegisterType(type.BaseType);

        var meta = _createTypeMetadataFor(type);
        _typeMetaMap[type] = meta;

        // Register named entities
        var asTypeName = meta.ASTypeName;
        if (asTypeName != null)
        {
            // Check for duplicates!
            if (_nameMetaMap.TryGetValue(asTypeName, out var conflictMeta))
                throw new ApplicationException($"Multiple classes are using AS type name {asTypeName}: trying to register {type} on top of {conflictMeta.ModelType}");

            _nameMetaMap[asTypeName] = meta;
        }
        
        // Register anonymous entities
        var entityType = meta.EntityType;
        if (entityType.IsAssignableTo(typeof(IAnonymousEntity)))
            _anonymousEntityTypes.Add(entityType);
        
        // Register nameless entities
        if (entityType.IsAssignableTo(typeof(INamelessEntity)))
            _namelessEntityTypes.Add(entityType);
    }

    private ModelMeta CreateModelMetaFor<TModel>()
        where TModel : ASType, IASModel<TModel>
    {
        var modelType = typeof(TModel);

        return new ModelMeta
        {
            ModelType = modelType,
            EntityTypeChain = GetEntityTypeChain<TModel>(modelType),
            EntityType = TModel.EntityType,
            ASTypeName = TModel.ASTypeName
        };
    }
    
    private List<Type> GetEntityTypeChain<TModel>(Type modelType)
        where TModel : ASType, IASModel<TModel>
    {
        var entityTypes = new List<Type>
        {
            TModel.EntityType
        };
        
        for (var baseType = modelType.BaseType; baseType != null; baseType = baseType.BaseType)
        {
            // Some base types may not be models.
            // If this happens, we need to skip but NOT bail out!
            if (!_typeMetaMap.TryGetValue(baseType, out var baseMeta))
                continue;
            
            entityTypes.Add(baseMeta.EntityType);
        }

        return entityTypes;
    }

    /// <summary>
    ///     Cached metadata about an AS Model.
    /// </summary>
    private class ModelMeta
    {
        /// <summary>
        ///     Type of the model class
        /// </summary>
        public required Type ModelType { get; init; }
        
        /// <summary>
        ///     Type of the entity class, taken from <see cref="IASModel{TModel}.EntityType"/>.
        /// </summary>
        public required Type EntityType { get; init; }
        
        /// <summary>
        ///     List of all entities required to represent the data used by this model.
        ///     Effectively, it is <see cref="EntityType"/> along with the entity type of every ancestor model.
        /// </summary>
        public required IReadOnlyList<Type> EntityTypeChain { get; init; }
        
        /// <summary>
        ///     The model's AS type name, taken from <see cref="IASModel{TModel}.ASTypeName"/>.
        /// </summary>
        public string? ASTypeName { get; init; }
    }
}