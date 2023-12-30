// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections;
using System.Text.Json.Serialization;
using ActivityPub.Types.Conversion.Converters;

namespace ActivityPub.Types.Util;

/// <summary>
///     A JSON-LD context.
///     Contains a set of context objects.
/// </summary>
/// <seealso cref="JsonLDContextObject" />
/// <seealso href="https://www.w3.org/TR/json-ld11/#dfn-context" />
/// <seealso href="https://www.w3.org/TR/json-ld11/#context-definitions" />
/// <seealso href="https://www.w3.org/TR/json-ld11/#the-context" />
public interface IJsonLDContext : IReadOnlyCollection<JsonLDContextObject>
{
    /// <inheritdoc cref="JsonLDContextObject.ActivityStreams"/>
    public static IJsonLDContext ActivityStreams { get; } = JsonLDContext.CreateASContext();
    
    /// <summary>
    ///     Parent context that this one derives from.
    ///     All context objects in the parent are automatically inherited by the child and will appear in <see cref="Contexts"/>.
    /// </summary>
    public IJsonLDContext? Parent { get; }
    
    /// <summary>
    ///     All context objects that either defined in this context, or inherited from the parent.
    /// </summary>
    public IEnumerable<JsonLDContextObject> Contexts { get; }
    
    /// <summary>
    ///     Checks if this context contains all objects from another.
    ///     In other words, returns <see langword="true"/> if this context is a proper superset of the provided context.
    /// </summary>
    public bool Contains(IJsonLDContext context);
    
    /// <summary>
    ///     Checks if this context contains a specified context object.
    /// </summary>
    public bool Contains(JsonLDContextObject contextObject);
    
    /// <summary>
    ///     Checks if a given term is defined by any context object within this context.
    /// </summary>
    public bool Contains(JsonLDTerm term);
}

/// <summary>
///     Mutable implementation of <see cref="IJsonLDContext"/>.
/// </summary>
[JsonConverter(typeof(JsonLDContextConverter))]
public class JsonLDContext : IJsonLDContext, ICollection<JsonLDContextObject>
{
    /// <summary>
    ///     Constructs a new context, pre-initialized with the ActivityStreams context.
    /// </summary>
    /// <seealso cref="JsonLDContextObject.ActivityStreams"/>
    public static JsonLDContext CreateASContext() => [JsonLDContextObject.ActivityStreams];
    
    /// <inheritdoc />
    public IJsonLDContext? Parent { get; private init; }

    /// <inheritdoc />
    public IEnumerable<JsonLDContextObject> Contexts
        => Parent != null
        ? LocalContexts.Concat(Parent.Contexts)
        : LocalContexts;
    
    private HashSet<JsonLDContextObject> LocalContexts { get; init; } = [];

    /// <summary>
    ///     Creates a new, empty context.
    /// </summary>
    public JsonLDContext() {}
    
    /// <summary>
    ///     Derives a new child context from the specified parent
    /// </summary>
    public JsonLDContext(IJsonLDContext parent)
        => Parent = parent;

    /// <summary>
    ///     Creates a shallow copy of this Json-LD context.
    /// </summary>
    public JsonLDContext Clone() => new()
    {
        Parent = Parent,
        LocalContexts = [..LocalContexts]
    };

    /// <summary>
    ///     Adds all context objects from the specified context.
    /// </summary>
    public void Add(IJsonLDContext context)
    {
        foreach (var contextObject in context)
        {
            Add(contextObject);
        }
    }
    
    /// <summary>
    ///     Adds a context object to this context
    /// </summary>
    public void Add(JsonLDContextObject contextObject)
    {
        if (Parent?.Contains(contextObject) != true)
            LocalContexts.Add(contextObject);
    }

    /// <summary>
    ///     Removes all local context objects from the context.
    ///     Objects in the parent are ignored.
    /// </summary>
    public void Clear() => LocalContexts.Clear();
    
    /// <inheritdoc cref="Contains(IJsonLDContext)" />
    public bool Contains(IJsonLDContext context)
        => context.All(Contains);

    /// <inheritdoc cref="Contains(JsonLDContextObject)" />
    public bool Contains(JsonLDContextObject contextObject)
        => LocalContexts.Contains(contextObject)
           || (Parent?.Contains(contextObject) ?? false);

    /// <inheritdoc cref="Contains(JsonLDTerm)" />
    public bool Contains(JsonLDTerm term)
        => this.Any(c => c.Contains(term));
    
    // Why do I have to implement this?
    /// <inheritdoc />
    public void CopyTo(JsonLDContextObject[] array, int arrayIndex)
    {
        if (arrayIndex + Count > array.Length)
            throw new ArgumentException("Not enough space in the target array", nameof(array));

        var i = arrayIndex;
        foreach (var contextObject in this)
        {
            array[i] = contextObject;
            i++;
        }
    }
    
    /// <summary>
    ///     Removes the specified context object from the context.
    ///     Only applies to local objects - inherited objects are ignored.
    /// </summary>
    public bool Remove(JsonLDContextObject item) => LocalContexts.Remove(item);

    /// <inheritdoc cref="ICollection{JsonLDContextObject}.Count" />
    public int Count
        => Parent != null
            ? LocalContexts.Count + Parent.Contexts.Count()
            : LocalContexts.Count;

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public IEnumerator<JsonLDContextObject> GetEnumerator() => Contexts.GetEnumerator();
    
    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}