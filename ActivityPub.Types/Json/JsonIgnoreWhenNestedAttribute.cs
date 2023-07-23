// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Json;

/// <summary>
/// Indicates that the property should be ignored when (de)serializing nested JSON objects
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class JsonIgnoreWhenNestedAttribute : Attribute {}