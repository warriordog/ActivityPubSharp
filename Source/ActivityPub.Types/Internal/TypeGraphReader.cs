// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using ActivityPub.Types.AS;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Internal;

internal interface ITypeGraphReader
{
    public JsonLDContext GetASContext();
    public List<string> GetASTypes();

    public bool TryReadEntity<TModel, TEntity>(TypeMap typeMap, [NotNullWhen(true)] out TEntity? entity)
        where TModel : ASType, IASModel<TModel, TEntity>
        where TEntity : ASEntity<TModel, TEntity>, new();

    public bool TryReadEntity<TModel>(TypeMap typeMap, [NotNullWhen(true)] out ASEntity? entity)
        where TModel : ASType, IASModel<TModel>;
}

internal class TypeGraphReader : ITypeGraphReader
{
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly JsonElement _sourceElement;

    private readonly JsonLDContext _context;
    private readonly HashSet<string> _asTypes;

    public TypeGraphReader(JsonSerializerOptions jsonOptions, JsonElement sourceElement)
    {
        _jsonOptions = jsonOptions;
        _sourceElement = sourceElement;

        if (sourceElement.ValueKind != JsonValueKind.Object)
            throw new ArgumentException($"{nameof(TypeGraphReader)} can only convert JSON objects. Input is of type {sourceElement.ValueKind}.", nameof(sourceElement));
        
        _context = ReadASContext(_sourceElement, _jsonOptions);
        _asTypes = ReadASTypes(_sourceElement, _jsonOptions);
    }

    public JsonLDContext GetASContext()
        => _context.Clone();

    public List<string> GetASTypes()
        => _asTypes.ToList();
    
    
    public bool TryReadEntity<TModel, TEntity>(TypeMap typeMap, [NotNullWhen(true)] out TEntity? entity)
        where TModel : ASType, IASModel<TModel, TEntity>
        where TEntity : ASEntity<TModel, TEntity>, new()
    {
        var shouldConvert = TModel.ShouldConvertFrom(_sourceElement, typeMap) ?? ShouldConvertObject<TModel>();
        if (shouldConvert)
        { 
            entity = _sourceElement.Deserialize<TEntity>(_jsonOptions)
                ?? throw new JsonException($"Failed to deserialize {TModel.EntityType} - JsonElement.Deserialize returned null");
            return true;
        }
        
        entity = null;
        return false;
    }
    
    public bool TryReadEntity<TModel>(TypeMap typeMap, [NotNullWhen(true)] out ASEntity? entity)
        where TModel : ASType, IASModel<TModel>
    {
        var shouldConvert = TModel.ShouldConvertFrom(_sourceElement, typeMap) ?? ShouldConvertObject<TModel>();
        if (shouldConvert)
        { 
            entity = (ASEntity?)_sourceElement.Deserialize(TModel.EntityType, _jsonOptions)
                ?? throw new JsonException($"Failed to deserialize {TModel.EntityType} - JsonElement.Deserialize returned null");
            return true;
        }
        
        entity = null;
        return false;
    }

    private bool ShouldConvertObject<TModel>()
        where TModel : ASType, IASModel<TModel>
    {
        // Check context first
        if (!_context.IsSupersetOf(TModel.DefiningContext))
            return false;
        
        // If this is a nameless entity, then we're done.
        if (TModel.ASTypeName == null)
            return true;
        
        // Next, we need to check the AS type name.
        if (_asTypes.Contains(TModel.ASTypeName))
            return true;

        // Finally, we need to check the AS type name of all *derived* types.
        // Otherwise, an object like {"type":"Create"} would not be detected as a type of Activity. 
        // This is non-trivial, as we must do this without reflection (this code is on the hot path).
        // Fortunately, most of the work is done for us by ASNameTree.
        var derivedTypes = TModel.DerivedTypeNames;
        return derivedTypes != null && _asTypes.Overlaps(derivedTypes);
    }

    private static JsonLDContext ReadASContext(JsonElement element, JsonSerializerOptions options)
    {
        // Try to get the context property.
        if (!element.TryGetProperty("@context", out var contextProp))
            // If missing then use default.
            return JsonLDContext.CreateASContext();

        // Convert context
        return contextProp.Deserialize<JsonLDContext>(options)
               ?? throw new JsonException("Can't convert TypeMap - \"@context\" property is null");
    }

    private static HashSet<string> ReadASTypes(JsonElement element, JsonSerializerOptions options)
    {
        // An object without the types field is just "object"
        if (!element.TryGetProperty("type", out var typeProp))
            return new HashSet<string>
            {
                ASObject.ObjectType
            };
        
        // Everything thing else must be converted
        var types = typeProp.Deserialize<HashSet<string>>(options) 
            ?? throw new JsonException("Can't convert TypeMap - \"type\" is null");
        types.Add(ASObject.ObjectType);
        return types;
    }
}