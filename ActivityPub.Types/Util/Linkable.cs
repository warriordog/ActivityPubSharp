// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using ActivityPub.Types.AS;

namespace ActivityPub.Types.Util;

/// <summary>
///     Synthetic wrapper for elements that can be included directly or referenced by a Link.
/// </summary>
/// <typeparam name="T">Type of element</typeparam>
public class Linkable<T>
    where T : ASObject
{
    public Linkable(ASLink link)
    {
        Link = link;
        HasLink = true;
    }

    public Linkable(T value)
    {
        Value = value;
        HasValue = true;
    }

    public Linkable(Linkable<T> linkable)
    {
        HasValue = linkable.HasValue;
        Value = linkable.Value;
    }

    [MemberNotNullWhen(true, nameof(Link))]
    [MemberNotNullWhen(false, nameof(Value))]
    public bool HasLink { get; }

    public ASLink? Link { get; }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Link))]
    public bool HasValue { get; }

    public T? Value { get; }

    public bool TryGetLink([NotNullWhen(true)] out ASLink? link)
    {
        if (HasLink)
        {
            link = Link!;
            return true;
        }

        link = null;
        return false;
    }

    public bool TryGetValue([NotNullWhen(true)] out T? value)
    {
        if (HasValue)
        {
            value = Value!;
            return true;
        }

        value = default;
        return false;
    }

    protected bool Equals(Linkable<T> other) => Equals(Link, other.Link) && EqualityComparer<T?>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        return Equals((Linkable<T>)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Link, Value);


    public static implicit operator Linkable<T>(ASLink link) => new(link);
    public static implicit operator Linkable<T>(ASUri link) => new((ASLink)link);
    public static implicit operator Linkable<T>(Uri link) => new((ASLink)link);
    public static implicit operator Linkable<T>(string link) => new((ASLink)link);
    public static implicit operator Linkable<T>(T value) => new(value);
    public static implicit operator ASLink?(Linkable<T>? linkable) => linkable?.Link;
    public static implicit operator T?(Linkable<T>? linkable) => linkable == null ? default : linkable.Value;
}