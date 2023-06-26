/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Util;

/// <summary>
/// Synthetic wrapper for elements that can be included directly or referenced by a Link.
/// </summary>
/// <typeparam name="T">Type of element</typeparam>
[JsonConverter(typeof(LinkableConverter))]
public class Linkable<T>
{
    [MemberNotNullWhen(true, nameof(Link))]
    public bool HasLink { get; }
    public ASLink? Link { get; }

    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue { get; }
    public T? Value { get; }

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

    protected bool Equals(Linkable<T> other) =>
        Equals(Link, other.Link) && EqualityComparer<T?>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Linkable<T>)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Link, Value);


    public static implicit operator Linkable<T>(ASLink link) => new(link);
    public static implicit operator Linkable<T>(T value) => new(value);
}