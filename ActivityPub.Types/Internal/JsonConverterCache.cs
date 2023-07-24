// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace ActivityPub.Types.Internal;

public class JsonConverterCache
{
    private readonly Dictionary<Type, JsonConverterFactory> _factoryInstances = new();
    private readonly Dictionary<ConverterCacheKey, JsonConverter> _converterInstances = new();
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    
    public JsonConverterCache(JsonSerializerOptions jsonSerializerOptions) => _jsonSerializerOptions = jsonSerializerOptions;

    public JsonConverter<T> GetConverterInstance<T>(Type converterType)
    {
        var typeToConvert = typeof(T);
        var key = new ConverterCacheKey(converterType, typeToConvert);
        
        // Check cache first
        if (_converterInstances.TryGetValue(key, out var converter))
            return (JsonConverter<T>)converter;
        
        // Next, try it as a factory
        if (converterType.IsAssignableTo(typeof(JsonConverterFactory)))
            return GetConverterInstanceFromFactory<T>(key);
        
        // Finally, see if it happens to match
        if (converterType.IsAssignableTo(typeof(JsonConverter<T>)))
            return GetConverterInstanceDirectly<T>(key);
        
        // We failed
        throw new JsonException($"Converter {converterType} is not a supported JsonConverter for {typeToConvert}");
    }

    private JsonConverter<T> GetConverterInstanceFromFactory<T>(ConverterCacheKey key)
    {
        // Get or create factory
        var factoryType = key.ConverterType;
        if (!_factoryInstances.TryGetValue(factoryType, out var factory))
        {
            factory = (JsonConverterFactory?)Activator.CreateInstance(factoryType);
            _factoryInstances[factoryType] = factory ?? throw new ApplicationException($"Activator.CreateInstance returned null for {factoryType}");
        }
        
        // Check compatibility
        if (!factory.CanConvert(key.TypeToConvert))
            throw new JsonException($"converter of type {factoryType} cannot convert values of type {key.TypeToConvert}");

        // Create and validate the instance
        var converter = factory.CreateConverter(key.TypeToConvert, _jsonSerializerOptions);
        if (converter == null)
            throw new JsonException($"converter of type {factoryType} returned null when asked to convert {key.TypeToConvert}");
        if (converter is not JsonConverter<T> typedConverter)
            throw new JsonException($"converter of type {factoryType} produced incompatible converter {converter.GetType()} for type {key.TypeToConvert}");

        // Cache and return it
        _converterInstances[key] = typedConverter;
        return typedConverter;
    }
    
    private JsonConverter<T> GetConverterInstanceDirectly<T>(ConverterCacheKey key)
    {
        var converter = (JsonConverter<T>?)Activator.CreateInstance(key.ConverterType);
        _converterInstances[key] = converter ?? throw new ApplicationException($"Activator.CreateInstance returned null for {key.ConverterType}");
        
        return converter;
    }
}

internal readonly record struct ConverterCacheKey(Type ConverterType, Type TypeToConvert);