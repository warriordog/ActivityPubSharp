namespace ActivityPub.Types.Util;

/// <summary>
/// Synthetic type to represent a list of T or Links to T
/// </summary>
public class LinkableList<T> : List<Linkable<T>>
where T : ASType
{
    public LinkableList() {}
    public LinkableList(int capacity) : base(capacity) {}
    public LinkableList(IEnumerable<Linkable<T>> collection) : base(collection) {}
    public LinkableList(IEnumerable<T> values)
    {
        AddRange(values);
    }
    public LinkableList(IEnumerable<ASLink> links)
    {
        AddRange(links);
    }

    public void Add(T value) => Add(new Linkable<T>(value));
    public void Add(ASLink link) => Add(new Linkable<T>(link));
    
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
}