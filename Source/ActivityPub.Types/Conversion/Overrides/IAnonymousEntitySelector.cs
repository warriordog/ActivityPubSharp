// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;

namespace ActivityPub.Types.Conversion.Overrides;


/// <summary>
///     Examines an incoming JSON message to determine which - if any - anonymous entities should be converted from it.
///     This is intended for entities that can't implement <see cref="IAnonymousEntity"/> directly.
/// </summary>
/// <seealso cref="IAnonymousEntity"/>
public interface IAnonymousEntitySelector
{
    /// <summary>
    ///     Checks a JSON message to identify all anonymous entities within it.
    /// </summary>
    /// <param name="inputJson">JSON message that is being converted. May not be an object.</param>
    public IEnumerable<Type> SelectAnonymousEntities(JsonElement inputJson);
}