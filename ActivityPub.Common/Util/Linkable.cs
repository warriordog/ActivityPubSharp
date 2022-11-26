using System.Diagnostics.CodeAnalysis;
using ActivityPub.Common.Types;

namespace ActivityPub.Common.Util;

// TODO override deserialization to map to/from a single string
// a plain string is equal to Link with only href set

/// <summary>
/// Synthetic wrapper for elements that can be included directly or referenced by a Link.
/// </summary>
/// <typeparam name="T">Type of element</typeparam>
public class Linkable<T>
{
    public bool HasLink => Link != null;
    public bool HasValue => Link == null;
    
    public ASLink? Link { get; }
    public T? Value { get; }

    public Linkable(ASLink link) => Link = link;
    public Linkable(T value) => Value = value;

    public bool TryGetLink([NotNullWhen(true)] out ASLink? link)
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

    protected bool Equals(Linkable<T> other) => Equals(Link, other.Link) && EqualityComparer<T?>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Linkable<T>)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Link, Value);
}