// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ActivityPub.Types.Conversion.Converters;

namespace ActivityPub.Types.Util;

/// <summary>
///     A string that can have multiple representations in different languages.
/// </summary>
[JsonConverter(typeof(NaturalLanguageStringConverter))]
public sealed class NaturalLanguageString
{
    /// <summary>
    ///     The language-agnostic value of this string.
    ///     Will be used as a fallback if the requested language is not available, or if no user preference is available.
    /// </summary>
    public string? DefaultValue
    {
        get => _rootNode.Value;
        set => _rootNode.Value = value;
    }
    private readonly LanguageNode _rootNode = new();

    /// <summary>
    ///     A map of BCP47 Language-Tags that represents this object.
    ///     <see cref="DefaultValue"/> is <em>not</em> included.
    /// </summary>
    public IReadOnlyDictionary<string, string> LanguageMap => _languageMap;
    private readonly Dictionary<string, string> _languageMap = new();
    
    /// <summary>
    ///     <see langword="true"/> if this string contains any language-tagged values.
    ///     <see cref="DefaultValue"/> is ignored.
    /// </summary>
    public bool HasLanguages => _languageMap.Any();
    
    /// <summary>
    ///     Constructs a NaturalLanguageString from a map of BCP47 Language-Tags.
    /// </summary>
    public static NaturalLanguageString FromLanguageMap(IReadOnlyDictionary<string, string> languageMap)
    {
        var langString = new NaturalLanguageString();
        foreach (var (key, value) in languageMap)
        {
            var language = key.Split("-");
            langString[language] = value;
        }
        return langString;
    }
    
    /// <summary>
    ///     Gets or sets the value of a string for a given language.
    /// </summary>
    /// <seealso cref="Get" />
    /// <seealso cref="Set" />
    public string? this[params string[] language]
    {
        get => Get(language);
        set
        {
            if (value != null)
                Set(value, language);
            else
                Remove(language);
        }
    }

    /// <summary>
    ///     Sets the value of the string for a given language.
    ///     If no language tags are specified, then sets the value of <see cref="DefaultValue" /> instead.
    /// </summary>
    /// <remarks>
    ///     The value must be provided <em>first</em> due to the use of <see langword="params" />.
    /// </remarks>
    /// <seealso cref="Remove"/>
    public void Set(string value, params string[] language)
    {
        var node = CreateNode(language);
        node.Value = value;

        var languageKey = string.Join('-', language);
        _languageMap[languageKey] = value;
    }


    /// <summary>
    ///     Gets the value of the string for a given language.
    ///     If no language tags are specified, then returns the value of <see cref="DefaultValue" /> instead.
    /// </summary>
    /// <seealso cref="GetExactly" />
    public string? Get(params string[] language)
    {
        var node = FindNode(language);
        return node?.Value;
    }

    /// <summary>
    ///     Gets the value of the string for a given language.
    ///     This version will not inherit from language roots or <see cref="DefaultValue"/>.
    /// </summary>
    /// <seealso cref="Get" />
    public string? GetExactly(params string[] language)
    {
        var node = FindNode(language, exactMatch: true);
        return node?.Value;
    }    
    
    /// <summary>
    ///     Checks if a value exists for the target language, or one of it's base categories.
    ///     This method does <em>not</em> consider <see cref="DefaultValue" />.
    /// </summary>
    /// <seealso cref="HasExactly"/>
    public bool Has(params string[] language)
    {
        var node = FindNode(language);
        return node?.Value != null;
    }

    /// <summary>
    ///     Checks if a value exists for the target language and all sub-tags.
    ///     This version will not inherit from language roots or <see cref="DefaultValue"/>.
    /// </summary>
    /// <seealso cref="Has"/>
    public bool HasExactly(params string[] language)
    {
        var node = FindNode(language, exactMatch: true);
        return node?.Value != null;
    }

    /// <summary>
    ///     Removes the mapping for a specific language.
    ///     This will not inherit from language roots or <see cref="DefaultValue"/>.
    /// </summary>
    /// <param name="language">Language to unmap</param>
    public void Remove(params string[] language)
    {
        var node = FindNode(language, exactMatch: true);
        if (node != null)
            node.Value = null;

        var languageKey = string.Join('-', language);
        _languageMap.Remove(languageKey);
    }

    /// <summary>
    ///     Constructs a <see cref="NaturalLanguageString"/> from a native .NET string.
    ///     The provided string will be mapped to <see cref="DefaultValue"/>.
    /// </summary>
    public static implicit operator NaturalLanguageString(string defaultValue) => new()
    {
        DefaultValue = defaultValue
    };

    private LanguageNode CreateNode(string[] language)
    {
        var node = _rootNode;
        foreach (var tag in language)
        {
            if (!node.TryGetSubTagNode(tag, out var nextNode))
            {
                nextNode = new LanguageNode();
                node.AddSubTagNode(tag, nextNode);
            }

            node = nextNode;
        }

        return node;
    }
    
    private LanguageNode? FindNode(string[] language, bool exactMatch = false)
    {
        var node = _rootNode;
        foreach (var tag in language)
        {
            if (node.TryGetSubTagNode(tag, out var nextNode))
            {
                node = nextNode;
                continue;
            }
            
            // If we can't go further, then we need to fail if we reach the end of the chain.
            if (exactMatch)
                return null;

            // Soft (non-exact) matching is allowed to bail out early
            break;
        }

        return node;
    }

    private sealed class LanguageNode
    {
        public string? Value { get; set; }

        private Dictionary<string, LanguageNode>? _subTags;

        public bool TryGetSubTagNode(string subTag, [NotNullWhen(true)] out LanguageNode? node)
        {
            if (_subTags != null)
                return _subTags.TryGetValue(subTag, out node);
            
            node = null;
            return false;
        }

        public void AddSubTagNode(string subTag, LanguageNode node)
        {
            _subTags ??= new Dictionary<string, LanguageNode>();
            _subTags.Add(subTag, node);
        }
    }
}