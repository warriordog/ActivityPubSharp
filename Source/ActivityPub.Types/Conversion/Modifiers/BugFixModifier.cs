// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization.Metadata;
using ActivityPub.Types.AS;
using ActivityPub.Types.Internal;

namespace ActivityPub.Types.Conversion.Modifiers;

/// <summary>
///     Custom type modifier to work around bugs in System.Text.Json.
/// </summary>
public static class BugFixModifier
{
    private static readonly HashSet<string> IgnoredPropNames = new()
    {
        nameof(ASEntity.ASTypeName),
        nameof(ASEntity.BaseTypeName),
        nameof(ASEntity.DefiningContext),
        nameof(ASEntity.RequiresObjectForm),
        nameof(IASModel<ASType>.EntityType)
    };

    /// <summary>
    ///     Adds all bugfix modifiers to the provided resolver.
    /// </summary>
    public static T WithBugFixes<T>(this T resolver)
        where T : DefaultJsonTypeInfoResolver
    {
        resolver.Modifiers.Add(HideIgnoredProperties);
        return resolver;
    }

    /// <summary>
    ///     Remove all public properties from <see cref="IASModel{TModel}"/> and <see cref="ASEntity"/>.
    ///     Works around <a href="https://github.com/dotnet/runtime/issues/50078">issue #50078</a>.
    /// </summary>
    public static void HideIgnoredProperties(JsonTypeInfo jsonTypeInfo)
    {
        // Only applies to objects
        if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        // Remove all of the inherited properties
        jsonTypeInfo.Properties.RemoveWhere(
            p => IgnoredPropNames.Contains(p.Name)
        );
    }
}