using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Common.Properties;

/// <summary>
/// Synthetic wrapper for elements that can be referred to by a link or object.
/// </summary>
/// <typeparam name="T">Type of object that can be referenced</typeparam>
public class LinkableProp<T>
{
    public bool HasLink => Link != null;
    public bool HasValue => Link == null;
    
    public string? Link { get; }
    public T? Value { get; }

    public LinkableProp(string link) => Link = link;
    public LinkableProp(T value) => Value = value;

    public bool TryGetLink([NotNullWhen(true)] out string? link)
    {
        if (Link != null)
        {
            link = Link;
            return true;
        }

        link = null;
        return false;
    }

    public bool TryGetValue([NotNullWhen(true)] out T? value)
    {
        if (Value != null)
        {
            value = Value;
            return true;
        }

        value = default;
        return false;
    }

    protected bool Equals(LinkableProp<T> other) => Link == other.Link && EqualityComparer<T?>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((LinkableProp<T>)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Link, Value);
}