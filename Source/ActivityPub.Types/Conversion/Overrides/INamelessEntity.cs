// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Util;

namespace ActivityPub.Types.Conversion.Overrides;

/// <summary>
///     Indicates that the entity's presence in an object cannot be determined by AS type name alone.
///     Instead, nameless entities are identified by the JSON-LD context. 
/// </summary>
/// <seealso cref="IAnonymousEntity"/>
public interface INamelessEntity
{
    /// <summary>
    ///     Checks if this entity should be included by the given JSON-LD context.
    /// </summary>
    public static abstract bool ShouldConvertFrom(IJsonLDContext jsonLDContext);
}