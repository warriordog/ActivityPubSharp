// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Util;

/// <summary>
/// A string that can be represented as either a single value or as a map from language tag to value.
/// </summary>
/// <remarks>
/// SingleString will be set to null whenever any language mappings are added.
/// 
/// It is possible for a NaturalLanguageString to have no value.
/// In this case, Get() will throw an exception.
/// To avoid this, call GetOrDefault()/GetOrNull() or check HasValue first.
/// </remarks>
[JsonConverter(typeof(NaturalLanguageStringConverter))]
public class NaturalLanguageString
{
    /// <summary>
    /// The language-indifferent value of this string.
    /// Will be null if there are any language mappings.
    /// </summary>
    /// <seealso cref="LanguageMap"/>
    public string? SingleString { get; private set; }

    /// <summary>
    /// The language-specific values of this string.
    /// Will be empty if there is an indifferent string value.
    /// </summary>
    /// <seealso cref="SingleString"/>
    public IReadOnlyDictionary<string, string> LanguageMap => _languageMap;

    private readonly Dictionary<string, string> _languageMap;

    /// <summary>
    /// True if this NaturalLanguageString has at least one value.
    /// </summary>
    public bool HasValue => SingleString != null || _languageMap.Any();

    /// <summary>
    /// Create a string from a single non-mapped value.
    /// </summary>
    /// <param name="singleString">Value of the string</param>
    public NaturalLanguageString(string? singleString)
    {
        SingleString = singleString;
        _languageMap = new Dictionary<string, string>();
    }

    /// <summary>
    /// Create a string from a language map.
    /// </summary>
    /// <remarks>
    /// Keys are language identifiers, values are the value of the string in that language.
    /// </remarks>
    /// <param name="languageMap">Language/value pairs to populate</param>
    public NaturalLanguageString(Dictionary<string, string> languageMap) => _languageMap = languageMap;

    /// <summary>
    /// Gets a value of the string.
    /// Will attempt to return a value in preferredLanguage, if provided.
    /// </summary>
    /// <remarks>
    /// 1st priority - unmapped string value
    /// 2nd priority - language matching the value of preferredLanguage
    /// 3rd priority - any mapped language
    /// fallback - throws InvalidOperationException
    /// </remarks>
    /// <param name="preferredLanguage">If not null, then attempt to return the value in the provided language.</param>
    /// <exception cref="InvalidOperationException">Throws InvalidOperationException if there are no defined values.</exception>
    /// <returns>Returns the best-compatible string</returns>
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

    /// <summary>
    /// Gets a value of the string.
    /// Will attempt return a value in one of preferredLanguages, if provided.
    /// A language value will be selected by the order of preferredLanguages.
    /// </summary>
    /// <remarks>
    /// 1st priority - unmapped string value
    /// 2nd priority - language matching the value of one of preferredLanguages
    /// 3rd priority - any mapped language
    /// fallback - throws InvalidOperationException
    /// </remarks>
    /// <param name="preferredLanguages">List of languages to prioritize</param>
    /// <exception cref="InvalidOperationException">Throws InvalidOperationException if there are no defined values.</exception>
    /// <returns>Returns the best-compatible string</returns>
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

    /// <summary>
    /// Gets a value of the string, or returns a provided default if no match can be found.
    /// Will attempt to return a value in preferredLanguage, if provided.
    /// </summary>
    /// <remarks>
    /// 1st priority - unmapped string value
    /// 2nd priority - language matching the value of preferredLanguage
    /// 3rd priority - any mapped language
    /// fallback - returns the value of def
    /// </remarks>
    /// <param name="def">Default to return in case no match is found.</param>
    /// <param name="preferredLanguage">If not null, then attempt to return the value in the provided language.</param>
    /// <returns>Returns the best-compatible string</returns>
    public string GetOrDefault(string def, string? preferredLanguage = null) => HasValue ? Get(preferredLanguage) : def;

    /// <summary>
    /// Gets a value of the string, or returns a provided default if no match can be found.
    /// Will attempt return a value in one of preferredLanguages, if provided.
    /// A language value will be selected by the order of preferredLanguages.
    /// </summary>
    /// <remarks>
    /// 1st priority - unmapped string value
    /// 2nd priority - language matching the value of one of preferredLanguages
    /// 3rd priority - any mapped language
    /// fallback - returns the value of def
    /// </remarks>
    /// <param name="def">Default to return in case no match is found.</param>
    /// <param name="preferredLanguages">List of languages to prioritize</param>
    /// <returns>Returns the best-compatible string</returns>
    public string GetOrDefault(string def, params string[] preferredLanguages) => HasValue ? Get(preferredLanguages) : def;

    /// <summary>
    /// Gets a value of the string.
    /// Will attempt to return a value in preferredLanguage, if provided.
    /// Returns null if there are no values.
    /// </summary>
    /// <remarks>
    /// 1st priority - unmapped string value
    /// 2nd priority - language matching the value of preferredLanguage
    /// 3rd priority - any mapped language
    /// fallback - null
    /// </remarks>
    /// <param name="preferredLanguage">If not null, then attempt to return the value in the provided language.</param>
    /// <returns>Returns the best-compatible string</returns>
    public string? GetOrNull(string? preferredLanguage = null) => HasValue ? Get(preferredLanguage) : null;

    /// <summary>
    /// Gets a value of the string.
    /// Will attempt return a value in one of preferredLanguages, if provided.
    /// A language value will be selected by the order of preferredLanguages.
    /// Returns null if there are no values.
    /// </summary>
    /// <remarks>
    /// 1st priority - unmapped string value
    /// 2nd priority - language matching the value of one of preferredLanguages
    /// 3rd priority - any mapped language
    /// fallback - null
    /// </remarks>
    /// <param name="preferredLanguages">List of languages to prioritize</param>
    /// <returns>Returns the best-compatible string</returns>
    public string? GetOrNull(params string[] preferredLanguages) => HasValue ? Get(preferredLanguages) : null;

    /// <summary>
    /// Gets or sets the content of the string.
    /// Provide a string to access the mapping for that language, or pass null to access the non-mapped value.
    /// </summary>
    /// <param name="language">Language to access, or null to use the non-mapped value</param>
    public string? this[string? language]
    {
        get
        {
            if (language == null) return SingleString;
            if (LanguageMap.TryGetValue(language, out var str)) return str;
            return null;
        }
        set
        {
            if (language == null) Set(value);
            else Set(language, value);
        }
    }

    /// <summary>
    /// Sets the string to single non-mapped value.
    /// All language mappings will be removed.
    /// </summary>
    /// <param name="value">New content of the string. Can be null.</param>
    public void Set(string? value)
    {
        SingleString = value;
        _languageMap.Clear();
    }

    /// <summary>
    /// Sets the mapping for a particular language.
    /// If value is null, then the mapping is deleted.
    /// This implicitly removes any non-mapped string that may be set.
    /// </summary>
    /// <param name="language">Language to map</param>
    /// <param name="value">Content of the string in the provided language</param>
    public void Set(string language, string? value)
    {
        SingleString = null;
        if (value != null) _languageMap[language] = value;
        else _languageMap.Remove(language);
    }

    /// <summary>
    /// Removes the mapping for a specific language
    /// </summary>
    /// <param name="language">Language to unmap</param>
    public void Remove(string language)
    {
        _languageMap.Remove(language);
    }

    public static implicit operator string?(NaturalLanguageString nls) => nls.GetOrNull();
    public static implicit operator NaturalLanguageString(string? str) => new(str);
}