// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Util;

/// <summary>
/// Internal interface to allow abstract, non-generic access through reflection
/// </summary>
internal interface ILinkableList
{
    internal int Count { get; }
    internal ILinkable Get(int i);
    internal void Set(int i, ASType value);
}

/// <summary>
/// Synthetic type to represent a list of T or Links to T
/// </summary>
public class LinkableList<T> : List<Linkable<T>>, ILinkableList
    where T : ASObject
{
    /// <summary>
    /// All link items within the collection.
    /// </summary>
    public IEnumerable<ASLink> LinkItems => this
        .Where(linkable => linkable.HasLink)
        .Select(linkable => linkable.Link!);

    /// <summary>
    /// All non-link items within the collection
    /// </summary>
    public IEnumerable<T> ValueItems => this
        .Where(linkable => linkable.HasValue)
        .Select(linkable => linkable.Value!);

    public LinkableList() {}
    public LinkableList(int capacity) : base(capacity) {}
    public LinkableList(IEnumerable<Linkable<T>> collection) : base(collection) {}
    public LinkableList(IEnumerable<T> values) => AddRange(values);
    public LinkableList(IEnumerable<ASLink> links) => AddRange(links);

    public void Add(T value) => Add(new Linkable<T>(value));
    public void Add(ASLink link) => Add(new Linkable<T>(link));
    public void Add(Linkable<T> linkable)
    {
        if (linkable.HasValue) Add(linkable.Value);
        else Add(linkable.Link);
    }

    public void AddRange(IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            Add(value);
        }
    }
    public void AddRange(IEnumerable<ASLink> links)
    {
        foreach (var link in links)
        {
            Add(link);
        }
    }
    public void AddRange(IEnumerable<Linkable<T>> linkables)
    {
        foreach (var linkable in linkables)
        {
            Add(linkable);
        }
    }
    
    ILinkable ILinkableList.Get(int i) => this[i];

    void ILinkableList.Set(int i, ASType value)
    {
        if (value is ASLink link)
            this[i] = link;
        else
            this[i] = (T)value;
    }
}