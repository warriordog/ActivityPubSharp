// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace InternalUtils;

public static class CollectionExtensions
{
    public static void AddRange<T>(this ISet<T> set, IEnumerable<T> items)
    {
        foreach (var item in items)
            set.Add(item);
    }

    public static void RemoveWhere<T>(this IList<T> list, Predicate<T> condition)
    {
        for (var i = list.Count - 1; i >= 0; i--)
        {
            if (condition(list[i]))
            {
                list.RemoveAt(i);
            }
        }
    }
}