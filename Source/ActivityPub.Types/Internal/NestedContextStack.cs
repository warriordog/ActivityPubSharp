// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Util;

namespace ActivityPub.Types.Internal;

internal class NestedContextStack
{
    private ThreadLocal<Stack<IJsonLDContext>> ContextStack { get; } = new
    (
        () => new Stack<IJsonLDContext>(),
        false
    );

    public void Push(IJsonLDContext context)
    {
        ContextStack.Value!.Push(context);
    }
    
    public IJsonLDContext? Peek()
    {
        ContextStack.Value!.TryPeek(out var value);
        return value;
    }

    public void Pop()
    {
        ContextStack.Value!.TryPop(out _);
    }
}