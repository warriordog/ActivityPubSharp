// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Conversion.Converters;

namespace ActivityPub.Types.Util;

/// <summary>
///     Synthetic type to represent a URI or IRI as used in ActivityStreams.
///     Currently, this only handles "true" non-compacted URIs.
/// </summary>
[JsonConverter(typeof(ASUriConverter))]
public class ASUri : IEquatable<ASUri>, IEquatable<Uri>, IEquatable<string>
{
    /// <summary>
    ///     Constructs an <see cref="ASUri"/> by wrapping an existing native <see cref="Uri"/>.
    /// </summary>
    public ASUri(Uri uri) => Uri = uri;
    
    /// <summary>
    ///     Constructs an <see cref="ASUri"/> by parsing a string.
    /// </summary>
    /// <param name="uri"></param>
    public ASUri(string uri) => Uri = new Uri(uri);
    
    /// <summary>
    ///     Parsed and expanded URI value of this object. 
    /// </summary>
    public Uri Uri { get; }

    /// <inheritdoc />
    public bool Equals(ASUri? other) => AreEqual(this, other);
    /// <inheritdoc />
    public bool Equals(string? other) => AreEqual(this, other);
    /// <inheritdoc />
    public bool Equals(Uri? other) => AreEqual(this, other);

    /// <inheritdoc />
    public override string ToString() => Uri.ToString();
    /// <inheritdoc />
    public override int GetHashCode() => Uri.GetHashCode();

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;

        return obj switch
        {
            ASUri asUri => Equals(asUri),
            Uri uri => Equals(uri),
            string str => Equals(str),
            _ => false
        };
    }

    /// <summary>
    ///     Compares two <see cref="ASUri"/> objects for equality.
    /// </summary>
    public static bool operator ==(ASUri? left, ASUri? right) => AreEqual(left, right);

    /// <summary>
    ///     Compares two <see cref="ASUri"/> objects for equality.
    /// </summary>
    public static bool operator !=(ASUri? left, ASUri? right) => !AreEqual(left, right);
    
    /// <summary>
    ///     Compares an <see cref="ASUri"/> against a native <see cref="Uri"/>.
    /// </summary>
    public static bool operator ==(ASUri? left, Uri? right) => AreEqual(left, right);
    
    /// <summary>
    ///     Compares an <see cref="ASUri"/> against a native <see cref="Uri"/>.
    /// </summary>
    public static bool operator !=(ASUri? left, Uri? right) => !AreEqual(left, right);

    /// <summary>
    ///     Compares an <see cref="ASUri"/> against a <see cref="string"/>.
    /// </summary>
    public static bool operator ==(ASUri? left, string? right) => AreEqual(left, right);
    
    /// <summary>
    ///     Compares an <see cref="ASUri"/> against a <see cref="string"/>.
    /// </summary>
    public static bool operator !=(ASUri? left, string? right) => !AreEqual(left, right);
    
    /// <summary>
    ///     Compares two <see cref="ASUri"/> objects for equality.
    /// </summary>
    public static bool AreEqual(ASUri? left, ASUri? right)
    {
        if (ReferenceEquals(left, right))
            return true;
        if (ReferenceEquals(null, right))
            return ReferenceEquals(null, left);
        if (ReferenceEquals(null, left))
            return ReferenceEquals(null, right);

        return left.Uri.Equals(right.Uri);
    }
    
    /// <summary>
    ///     Compares an <see cref="ASUri"/> against a native <see cref="Uri"/>.
    /// </summary>
    public static bool AreEqual(ASUri? left, Uri? right)
    {
        if (ReferenceEquals(null, right))
            return ReferenceEquals(null, left);
        if (ReferenceEquals(null, left))
            return ReferenceEquals(null, right);

        return left.Uri.Equals(right);
    }
    
    /// <summary>
    ///     Compares an <see cref="ASUri"/> against a <see cref="string"/>.
    /// </summary>
    public static bool AreEqual(ASUri? left, string? right)
    {
        if (ReferenceEquals(null, right))
            return ReferenceEquals(null, left);
        if (ReferenceEquals(null, left))
            return ReferenceEquals(null, right);

        // This works - Uri.Equals has a built-in special case for strings
        // ReSharper disable once SuspiciousTypeConversion.Global
        return left.Uri.Equals(right);
    }
    /// <summary>
    ///     Parses an <see cref="ASUri"/> from a <see cref="string"/>
    /// </summary>
    public static implicit operator string(ASUri asUri) => asUri.ToString();

    /// <summary>
    ///     Converts this <see cref="ASUri"/> to its <see cref="string"/> value
    /// </summary>
    public static implicit operator ASUri(string str) => new(str);

    /// <summary>
    ///     Converts a native <see cref="Uri"/> into a an <see cref="ASUri"/>
    /// </summary>
    public static implicit operator ASUri(Uri uri) => new(uri);
    
    /// <summary>
    ///     Converts an <see cref="ASUri"/> into a native <see cref="Uri"/>
    /// </summary>
    public static implicit operator Uri(ASUri asUri) => asUri.Uri;
}