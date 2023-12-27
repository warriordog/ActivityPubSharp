// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Internal;

/// <summary>
///     Represents the AS type name of a composite object.
///     More-specific subtypes will "shadow" (replace) more-generic base types. 
/// </summary>
internal class CompositeASType
{
    private readonly HashSet<string> _allASTypes = new();
    private readonly HashSet<string> _flatASTypes = new();
    private readonly HashSet<string> _replacedASTypes = new();

    /// <summary>
    ///     AS type names that are represented by this object, excluding those that have been shadowed.
    ///     The returned object is a live, read-only view of the collection.
    ///     Changes will be reflected immediately.
    /// </summary>
    public IReadOnlySet<string> Types => _flatASTypes;

    /// <summary>
    ///     AS type names that are represented by this object, including those that have been shadowed.
    ///     The returned object is a live, read-only view of the collection.
    ///     Changes will be reflected immediately.
    /// </summary>
    public IReadOnlySet<string> AllTypes => _allASTypes;

    /// <summary>
    ///     Adds a new type.
    /// </summary>
    /// <param name="type">Name of the type to add</param>
    /// <param name="replacedType">Optional, name of the type that is replaced by this one</param>
    public void Add(string type, string? replacedType = null)
    {
        // Add it to the superset.
        // This may happen repeatedly as a type graph is hydrated.
        _allASTypes.Add(type);
        
        // Replace the base type
        if (replacedType != null)
        {
            _replacedASTypes.Add(replacedType);
            _flatASTypes.Remove(replacedType);
        }

        // Add the new type, if it's not flattened away
        if (!_replacedASTypes.Contains(type))
            _flatASTypes.Add(type);
    }
    
    /// <summary>
    ///     Adds a collection of type names.
    ///     These are assumed to to not shadow anything, but may be shadowed by existing types.
    /// </summary>
    public void AddRange(IEnumerable<string> asTypes)
    {
        foreach (var asType in asTypes)
        {
            Add(asType);
        }
    }
}