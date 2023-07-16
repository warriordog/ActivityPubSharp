// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;
using System.Text.Json.Serialization;

namespace ActivityPub.Types.Internal.TypeInfo;

/// <summary>
/// Cached details about a particular JSON property
/// </summary>
public class JsonPropertyInfo
{
    public required PropertyInfo Property { get; init; }
    public required string Name { get; init; }
    public required bool IsRequired { get; init; }
    
    /// <summary>
    /// Property-specific ignore condition.
    /// Will never be <see cref="JsonIgnoreCondition.Always"/>.
    /// </summary>
    public required JsonIgnoreCondition? IgnoreCondition { get; init; }
    public required object? TypeDefaultValue { get; init; }
    public required bool IsCollection { get; init; }
}