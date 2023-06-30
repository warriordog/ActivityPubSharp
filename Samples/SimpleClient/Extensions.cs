// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SimpleClient;

public static class Extensions
{
    public static async Task<TList> ToList<TList, TItem>(this IAsyncEnumerable<TItem> stream, TList list)
        where TList : IList<TItem>
    {
        await foreach (var item in stream)
        {
            list.Add(item);
        }

        return list;
    }
}