// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using ActivityPub.Types.AS;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Internal;

internal interface ITypeGraphReader
{
    public JsonLDContext ASContext { get; }
    public CompositeASType ASTypes { get; }

    public bool TryReadEntity<TModel, TEntity>(TypeMap typeMap, [NotNullWhen(true)] out TEntity? entity)
        where TModel : ASType, IASModel<TModel, TEntity>
        where TEntity : ASEntity<TModel, TEntity>, new();

    public bool TryReadEntity<TModel>(TypeMap typeMap, [NotNullWhen(true)] out ASEntity? entity)
        where TModel : ASType, IASModel<TModel>;
}

internal class TypeGraphReader : ITypeGraphReader
{
    public JsonLDContext ASContext { get; }
    public CompositeASType ASTypes { get; }

    private readonly JsonSerializerOptions _jsonOptions;
    private readonly JsonElement _sourceElement;
    private readonly NestedContextStack _nestedContextStack;

    public TypeGraphReader(JsonSerializerOptions jsonOptions, JsonElement sourceElement, NestedContextStack nestedContextStack)
    {
        _jsonOptions = jsonOptions;
        _sourceElement = sourceElement;
        _nestedContextStack = nestedContextStack;

        if (sourceElement.ValueKind != JsonValueKind.Object)
            throw new ArgumentException($"{nameof(TypeGraphReader)} can only convert JSON objects. Input is of type {sourceElement.ValueKind}.", nameof(sourceElement));
        
        ASContext = ReadASContext(_sourceElement, _jsonOptions);
        ASTypes = ReadASTypes(_sourceElement, _jsonOptions);
    }
    
    public bool TryReadEntity<TModel, TEntity>(TypeMap typeMap, [NotNullWhen(true)] out TEntity? entity)
        where TModel : ASType, IASModel<TModel, TEntity>
        where TEntity : ASEntity<TModel, TEntity>, new()
    {
        try
        {
            _nestedContextStack.Push(ASContext);
            
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
        finally
        {
            _nestedContextStack.Pop();
        }
    }
    
    public bool TryReadEntity<TModel>(TypeMap typeMap, [NotNullWhen(true)] out ASEntity? entity)
        where TModel : ASType, IASModel<TModel>
    {
        try
        {
            _nestedContextStack.Push(ASContext);
            
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
        finally
        {
            _nestedContextStack.Pop();
        }
    }

    private bool ShouldConvertObject<TModel>()
        where TModel : ASType, IASModel<TModel>
    {
        // Check context first
        if (!ASContext.Contains(TModel.DefiningContext))
            return false;
        
        // If this is a nameless entity, then we're done.
        if (TModel.ASTypeName == null)
            return true;
        
        // Next, we need to check the AS type name.
        if (ASTypes.Contains(TModel.ASTypeName))
            return true;

        // Finally, we need to check the AS type name of all *derived* types.
        // Otherwise, an object like {"type":"Create"} would not be detected as a type of Activity. 
        // This is non-trivial, as we must do this without reflection (this code is on the hot path).
        // Fortunately, most of the work is done for us by ASNameTree.
        var derivedTypes = TModel.DerivedTypeNames;
        return derivedTypes != null && ASTypes.AllTypes.Overlaps(derivedTypes);
    }

    private JsonLDContext ReadASContext(JsonElement element, JsonSerializerOptions options)
    {
        var parentContext = _nestedContextStack.Peek();
        
        // Try to get the context property.
        if (!element.TryGetProperty("@context", out var contextProp))
            // If missing then use default.
            return JsonLDContext.CreateASContext(parentContext);

        // Convert context
        var context = contextProp.Deserialize<JsonLDContext>(options)
               ?? throw new JsonException("Can't convert TypeMap - \"@context\" property is null");
        context.Add(JsonLDContextObject.ActivityStreams);
        context.SetParent(parentContext);
        return context;
    }

    private static CompositeASType ReadASTypes(JsonElement element, JsonSerializerOptions options)
    {
        // An object without the types field is just "object"
        if (!element.TryGetProperty("type", out var typeProp))
            return [ASObject.ObjectType];
        
        // Everything thing else must be converted
        var allTypes = typeProp.Deserialize<HashSet<string>>(options) 
            ?? throw new JsonException("Can't convert TypeMap - \"type\" is null");

        var compositeType = new CompositeASType();
        compositeType.AddRange(allTypes);
        compositeType.Add(ASObject.ObjectType);
        return compositeType;
    }
}