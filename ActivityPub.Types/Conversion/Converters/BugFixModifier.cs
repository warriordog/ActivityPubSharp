// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization.Metadata;
using ActivityPub.Types.AS;

namespace ActivityPub.Types.Conversion.Converters;

/// <summary>
///     Custom type modifier to work around bugs in System.Text.Json.
/// </summary>
public static class BugFixModifier
{
    /// <summary>
    ///     Adds all bugfix modifiers to the provided resolver.
    /// </summary>
    public static T WithBugFixes<T>(this T resolver)
        where T : DefaultJsonTypeInfoResolver
    {
        resolver.Modifiers.Add(HideASEntity);
        return resolver;
    }

    /// <summary>
    ///     Remove all public properties from <see cref="ASEntity"/> and <see cref="ASEntity{T}"/>.
    ///     Works around <a href="https://github.com/dotnet/runtime/issues/50078">issue #50078</a>.
    /// </summary>
    public static void HideASEntity(JsonTypeInfo jsonTypeInfo)
    {
        // Only applies to objects
        if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        // Search through all included properties
        var properties = jsonTypeInfo.Properties;
        for (var i = properties.Count - 1; i >= 0; i--)
            // Match any that are inherited AND the base has JsonIgnore
            if (IsIgnoreProp(properties[i].Name))
                // Remove all matched
                properties.RemoveAt(i);
    }

    // Ugly hack
    private static bool IsIgnoreProp(string name)
    {
        if (name.Equals(nameof(ASEntity.ASTypeName), StringComparison.OrdinalIgnoreCase))
            return true;

        if (name.Equals(nameof(ASEntity.ReplacesASTypes), StringComparison.OrdinalIgnoreCase))
            return true;

        if (name.Equals(nameof(ASEntity.TypeMap), StringComparison.OrdinalIgnoreCase))
            return true;

        if (name.Equals(nameof(ILinkEntity.RequiresObjectForm), StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}