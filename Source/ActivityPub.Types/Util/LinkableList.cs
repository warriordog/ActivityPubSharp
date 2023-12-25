// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.AS;

namespace ActivityPub.Types.Util;

/// <summary>
///     Synthetic type to represent a list of <code>T</code> or Links to <code>T</code>
/// </summary>
public class LinkableList<T> : List<Linkable<T>>
    where T : ASType
{
    /// <inheritdoc />
    public LinkableList() {}
    
    /// <inheritdoc />
    public LinkableList(int capacity) : base(capacity) {}
    
    /// <inheritdoc />
    public LinkableList(IEnumerable<Linkable<T>> collection) : base(collection) {}
    
    /// <inheritdoc cref="LinkableList{T}(IEnumerable{Linkable{T}})" />
    public LinkableList(IEnumerable<T> values) => AddRange(values);
    
    /// <inheritdoc cref="LinkableList{T}(IEnumerable{Linkable{T}})" />
    public LinkableList(IEnumerable<ASLink> links) => AddRange(links);
    
    /// <summary>
    ///     All link items within the collection.
    /// </summary>
    public IEnumerable<ASLink> LinkItems => this
        .Where(linkable => linkable.HasLink)
        .Select(linkable => linkable.Link!);

    /// <summary>
    ///     All non-link items within the collection
    /// </summary>
    public IEnumerable<T> ValueItems => this
        .Where(linkable => linkable.HasValue)
        .Select(linkable => linkable.Value!);

    /// <summary>
    ///     Adds multiple values to the list
    /// </summary>
    public void AddRange(IEnumerable<T> values)
    {
        foreach (var value in values)
            Add(value);
    }

    /// <summary>
    ///     Adds multiple links to the list
    /// </summary>
    public void AddRange(IEnumerable<ASLink> links)
    {
        foreach (var link in links)
            Add(link);
    }

    // These are required for weird type resolution reasons.
    // If removed, other code wont compile.
    
    /// <inheritdoc cref="List{T}.Add(T)"/>
    public new void Add(Linkable<T> linkable) => base.Add(linkable);
    
    /// <inheritdoc cref="List{T}.AddRange(IEnumerable{T})"/>
    public new void AddRange(IEnumerable<Linkable<T>> linkables) => base.AddRange(linkables);

    /// <summary>
    ///     Constructs a linkable list from a single link
    /// </summary>
    public static implicit operator LinkableList<T>(ASLink link) => new() { link };

    /// <summary>
    ///     Constructs a linkable list from a single link URI
    /// </summary>
    public static implicit operator LinkableList<T>(ASUri link) => new() { (ASLink)link };

    /// <summary>
    ///     Constructs a linkable list from a single link URI
    /// </summary>
    public static implicit operator LinkableList<T>(Uri link) => new() { (ASLink)link };

    /// <summary>
    ///     Constructs a linkable list from a single link URI
    /// </summary>
    public static implicit operator LinkableList<T>(string link) => new() { (ASLink)link };

    /// <summary>
    ///     Constructs a linkable list from a single value
    /// </summary>
    public static implicit operator LinkableList<T>(T value) => new() { value };

    /// <summary>
    ///     Constructs a linkable list from a single linkable
    /// </summary>
    public static implicit operator LinkableList<T>(Linkable<T> linkable) => new() { linkable };
    
    /// <summary>
    ///     Constructs a linkable list from a list of values
    /// </summary>
    public static implicit operator LinkableList<T>(List<T> values) => new(values);
    
    /// <summary>
    ///     Constructs a linkable list from a list of links
    /// </summary>
    public static implicit operator LinkableList<T>(List<ASLink> values) => new(values);
}