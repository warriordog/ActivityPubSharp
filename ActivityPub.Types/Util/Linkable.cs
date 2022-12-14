using System.Diagnostics.CodeAnalysis;
using ActivityPub.Types.Json;
using Newtonsoft.Json;

namespace ActivityPub.Types.Util;

/// <summary>
/// Synthetic wrapper for elements that can be included directly or referenced by a Link.
/// </summary>
/// <typeparam name="T">Type of element</typeparam>
[JsonConverter(typeof(LinkableConverter))]
public class Linkable<T> : ILinkable
where T : ASType
{
    public bool HasLink { get; }

    private readonly ASLink? _link;
    
    public bool HasValue { get; }
    private readonly T? _value;

    public Linkable(ASLink link)
    {
        _link = link;
        HasLink = true;
    }
    public Linkable(T value)
    {
        _value = value;
        HasValue = true;
    }

    public bool TryGetLink([NotNullWhen(true)] out ASLink? link)
    {
        if (HasLink)
        {
            link = _link!;
            return true;
        }

        link = null;
        return false;
    }

    public bool TryGetValue([NotNullWhen(true)] out T? value)
    {
        if (HasValue)
        {
            value = _value!;
            return true;
        }

        value = default;
        return false;
    }

    public ASType GetValue() => ((ASType?)_link ?? _value)!;

    protected bool Equals(Linkable<T> other) => Equals(_link, other._link) && EqualityComparer<T?>.Default.Equals(_value, other._value);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Linkable<T>)obj);
    }

    public override int GetHashCode() => HashCode.Combine(_link, _value);
    

    public static implicit operator Linkable<T>(ASLink link) => new(link);
    public static implicit operator Linkable<T>(T value) => new(value);
}

public interface ILinkable
{
    public ASType GetValue();
    
}