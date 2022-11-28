using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Common.Util;

/// <summary>
/// A string that can be represented as either a single value or as a map from language tag to value.
/// </summary>
/// <remarks>
/// It is possible for a NaturalLanguageString to have no value.
/// In this case, Get() will throw an exception.
/// To avoid this, call GetOrDefault()/GetOrNull() or check HasValue.
/// </remarks>
public class NaturalLanguageString
{
    public string? SingleString { get; private set; }
    
    public IReadOnlyDictionary<string, string> LanguageMap => _languageMap;
    private readonly Dictionary<string, string> _languageMap;

    public bool HasValue => SingleString != null || _languageMap.Any();

    public NaturalLanguageString(string singleString)
    {
        SingleString = singleString;
        _languageMap = new Dictionary<string, string>();
    }

    public NaturalLanguageString(Dictionary<string, string> languageMap) => _languageMap = languageMap;

    public string Get(string? preferredLanguage = null)
    {
        if (SingleString != null)
        {
            return SingleString;
        }

        if (preferredLanguage != null && _languageMap.TryGetValue(preferredLanguage, out var preferredString))
        {
            return preferredString;
        }

        return _languageMap.First().Value;
    }

    public string Get(params string[] preferredLanguages)
    {
        if (SingleString != null)
        {
            return SingleString;
        }

        foreach (var preferredLanguage in preferredLanguages)
        {
            if (_languageMap.TryGetValue(preferredLanguage, out var preferredString))
            {
                return preferredString;
            }
        }

        return _languageMap.First().Value;
    }

    public string GetOrDefault(string def, string? preferredLanguage = null) => HasValue ? Get(preferredLanguage) : def;
    public string GetOrDefault(string def, params string[] preferredLanguages) => HasValue ? Get(preferredLanguages) : def;

    public string? GetOrNull(string? preferredLanguage = null) => HasValue ? Get(preferredLanguage) : null;
    public string? GetOrNull(params string[] preferredLanguages) => HasValue ? Get(preferredLanguages) : null;
    
    public void Set(string value)
    {
        SingleString = value;
        _languageMap.Clear();
    }

    public void Set(string language, string value)
    {
        SingleString = null;
        _languageMap[language] = value;
    }

    public void Remove(string language)
    {
        _languageMap.Remove(language);
    }
    
    // TODO implicit cast from string
}