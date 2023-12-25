// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using ActivityPub.Types.AS;

namespace ActivityPub.Types.Util;

/// <summary>
///     Synthetic wrapper for elements that can be included directly or referenced by a Link.
/// </summary>
/// <typeparam name="T">Type of element</typeparam>
public sealed class Linkable<T>
    where T : ASType
{
    /// <summary>
    ///     Creates a Linkable from a reference link
    /// </summary>
    public Linkable(ASLink link)
    {
        Link = link;
        HasLink = true;
    }

    /// <summary>
    ///     Creates a linkable from a value object
    /// </summary>
    public Linkable(T value)
    {
        Value = value;
        HasValue = true;
    }

    /// <summary>
    ///     Creates a linkable by cloning another linkable
    /// </summary>
    public Linkable(Linkable<T> linkable)
    {
        HasValue = linkable.HasValue;
        Value = linkable.Value;
    }

    /// <summary>
    ///     <see langword="true"/> if this Linkable has a link reference
    /// </summary>
    [MemberNotNullWhen(true, nameof(Link))]
    [MemberNotNullWhen(false, nameof(Value))]
    public bool HasLink { get; }

    /// <summary>
    ///     The link reference, or <see langword="null"/> if this linkable has an object.
    /// </summary>
    public ASLink? Link { get; }

    /// <summary>
    ///     <see langword="true"/> if this Linkable has a value object
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Link))]
    public bool HasValue { get; }

    /// <summary>
    ///     The value object, or <see langword="this"/> if this Linkable has a link reference
    /// </summary>
    public T? Value { get; }

    /// <summary>
    ///     If this Linkable has a link, then returns <see langword="true"/> and assigns it to "link".
    ///     Otherwise, returns <see langword="false"/>.
    /// </summary>
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

    /// <summary>
    ///     If this Linkable has a value, then returns <see langword="true"/> and assigns it to "value".
    ///     Otherwise, returns <see langword="false"/>.
    /// </summary>
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

    private bool Equals(Linkable<T> other) => Equals(Link, other.Link) && EqualityComparer<T?>.Default.Equals(Value, other.Value);

    /// <inheritdoc />
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

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Link, Value);


    /// <inheritdoc cref="Linkable{T}(ASLink)"/>
    public static implicit operator Linkable<T>(ASLink link) => new(link);
    
    /// <inheritdoc cref="Linkable{T}(ASLink)"/>
    public static implicit operator Linkable<T>(ASUri link) => new((ASLink)link);
    
    /// <inheritdoc cref="Linkable{T}(ASLink)"/>
    public static implicit operator Linkable<T>(Uri link) => new((ASLink)link);
    
    /// <inheritdoc cref="Linkable{T}(ASLink)"/>
    public static implicit operator Linkable<T>(string link) => new((ASLink)link);
    
    /// <inheritdoc cref="Linkable{T}(T)"/>
    public static implicit operator Linkable<T>(T value) => new(value);
    
    /// <summary>
    ///     Converts this <see cref="Linkable{T}"/> to its reference link, or <see langword="null"/> if it has a value object
    /// </summary>
    public static implicit operator ASLink?(Linkable<T>? linkable) => linkable?.Link;
    
    /// <summary>
    ///     Converts this <see cref="Linkable{T}"/> to its value object, or <see langword="null"/> if it has a reference link
    /// </summary>
    public static implicit operator T?(Linkable<T>? linkable) => linkable == null ? default : linkable.Value;
}