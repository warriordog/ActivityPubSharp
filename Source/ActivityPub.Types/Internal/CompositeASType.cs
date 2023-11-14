// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Internal;

/// <summary>
///     Represents the AS type name of a composite object.
///     More-specific subtypes will "shadow" (replace) more-generic base types. 
/// </summary>
public class CompositeASType
{
    // TODO consider adding "all"

    private readonly HashSet<string> _asTypes = new();
    private readonly HashSet<string> _replacedASTypes = new();

    /// <summary>
    ///     AS type names that are represented by this object, excluding those that have been shadowed.
    /// </summary>
    public IReadOnlySet<string> Types => _asTypes;

    /// <summary>
    ///     Adds a new type.
    /// </summary>
    /// <param name="type">Name of the type to add</param>
    /// <param name="replacedType">Optional, name of the type that is replaced by this one</param>
    public void Add(string type, string? replacedType = null)
    {
        // Replace the base type
        if (replacedType != null)
        {
            _replacedASTypes.Add(replacedType);
            _asTypes.Remove(replacedType);
        }

        // Add the new type
        if (!_replacedASTypes.Contains(type))
            _asTypes.Add(type);
    }
}