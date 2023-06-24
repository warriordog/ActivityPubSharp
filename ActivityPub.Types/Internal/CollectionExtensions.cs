// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Internal;

internal static class CollectionExtensions
{
    /// <summary>
    /// Removes from a list all elements that satisfy a predicate.
    /// </summary>
    /// <param name="list">List to remove from</param>
    /// <param name="predicate">Predicate that returns true if the element should be removed</param>
    /// <typeparam name="T">Type of element</typeparam>
    public static void RemoveWhere<T>(this IList<T> list, Predicate<T> predicate)
    {
        // Count in reverse to avoid missing anything.
        // When we delete, everything shifts left (downward).
        for (var i = list.Count - 1; i >= 0; i--)
        {
            var value = list[i];
            if (predicate(value))
            {
                list.RemoveAt(i);
            }
        }
    }
}