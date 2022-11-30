using ActivityPub.Common.Types;

namespace ActivityPub.Serialization.Mapping;

public interface IASTypeMapper
{
    public Type FallbackType { get; set; }
    public Type GetTypeByNames(params string[] asTypeNames);
    public Type GetTypeByName(string asTypeName);
    public void MapNameToType<T>(string asTypeName) where T : ASType;
}

public class ASTypeMapper : IASTypeMapper
{
    private readonly Dictionary<string, TypeMapEntry> _typeMap = new();

    public Type FallbackType { get; set; } = typeof(ASType);

    public Type GetTypeByNames(params string[] asTypeNames)
    {
        
    }

    public Type GetTypeByName(string asTypeName)
    {
        if (_typeMap.TryGetValue(asTypeName, out var typeEntry))
        {
            return typeEntry.BackingType;
        }

        return FallbackType;
    }

    public void MapNameToType<T>(string asTypeName)
        where T : ASType
    {
        if (_typeMap.ContainsKey(asTypeName))
        {
            throw new InvalidOperationException($"Attempting to map duplicate ActivityStreams type {asTypeName}");
        }

        // Add mapping for type
        var backingType = typeof(T);
        _typeMap[asTypeName] = new TypeMapEntry(backingType);
        
        // Record it in any base types
        for (var baseType = backingType.BaseType; baseType != null; baseType = baseType.BaseType)
        {
            var baseTypeEntry = _typeMap.Values.FirstOrDefault(e => e.BackingType == baseType);
            baseTypeEntry?.NarrowerTypes.Add(asTypeName);
        }
    }
}

internal class TypeMapEntry
{
    public readonly Type BackingType;
    public readonly List<string> NarrowerTypes = new();

    public TypeMapEntry(Type backingType) => BackingType = backingType;
}