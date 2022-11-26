namespace ActivityPub.Common.Properties;

public class LinkablePropList<T> : List<LinkableProp<T>>
{
    public LinkablePropList() {}
    public LinkablePropList(int capacity) : base(capacity) {}
    public LinkablePropList(IEnumerable<LinkableProp<T>> collection) : base(collection) {}
    public LinkablePropList(IEnumerable<T> values)
    {
        AddRange(values);
    }
    public LinkablePropList(IEnumerable<string> links)
    {
        AddRange(links);
    }

    public void Add(T value) => Add(new LinkableProp<T>(value));
    public void Add(string link) => Add(new LinkableProp<T>(link));
    
    public void AddRange(IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            Add(value);
        }
    }
    public void AddRange(IEnumerable<string> links)
    {
        foreach (var link in links)
        {
            Add(link);
        }
    }
}