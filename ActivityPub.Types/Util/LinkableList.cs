// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Util;

/// <summary>
///     Synthetic type to represent a list of T or Links to T
/// </summary>
public class LinkableList<T> : List<Linkable<T>>
    where T : ASObject
{
    public LinkableList() {}
    public LinkableList(int capacity) : base(capacity) {}
    public LinkableList(IEnumerable<Linkable<T>> collection) : base(collection) {}
    public LinkableList(IEnumerable<T> values) => AddRange(values);
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

    public void Add(T value) => Add(new Linkable<T>(value));
    public void Add(ASLink link) => Add(new Linkable<T>(link));

    public void AddRange(IEnumerable<T> values)
    {
        foreach (var value in values)
            Add(value);
    }

    public void AddRange(IEnumerable<ASLink> links)
    {
        foreach (var link in links)
            Add(link);
    }

    // These are  required for weird type resolution reasons.
    // If removed, other code wont compile.
    public new void Add(Linkable<T> linkable) => base.Add(linkable);
    public new void AddRange(IEnumerable<Linkable<T>> linkables) => base.AddRange(linkables);

    public static implicit operator LinkableList<T>(ASLink link) => new() { link };
    public static implicit operator LinkableList<T>(ASUri link) => new() { (ASLink)link };
    public static implicit operator LinkableList<T>(Uri link) => new() { (ASLink)link };
    public static implicit operator LinkableList<T>(string link) => new() { (ASLink)link };
    public static implicit operator LinkableList<T>(T value) => new() { value };
    public static implicit operator LinkableList<T>(Linkable<T> linkable) => new() { linkable };

    public static implicit operator LinkableList<T>(List<T> values) => new(values);
    public static implicit operator LinkableList<T>(List<ASLink> values) => new(values);
}