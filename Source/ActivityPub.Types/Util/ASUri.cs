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
    ///     Constructs an ASUri by wrapping an existing native Uri.
    /// </summary>
    public ASUri(Uri uri) => Uri = uri;
    
    /// <summary>
    ///     Constructs an ASUri by parsing a string.
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

        if (obj is ASUri asUri)
            return Equals(asUri);
        if (obj is Uri uri)
            return Equals(uri);
        if (obj is string str)
            return Equals(str);

        return false;
    }

    /// <summary>
    ///     Compares two ASUri objects for equality.
    /// </summary>
    public static bool operator ==(ASUri? left, ASUri? right) => AreEqual(left, right);

    /// <summary>
    ///     Compares two ASUri objects for equality.
    /// </summary>
    public static bool operator !=(ASUri? left, ASUri? right) => !AreEqual(left, right);
    
    /// <summary>
    ///     Compares an ASUri against a native Uri.
    /// </summary>
    public static bool operator ==(ASUri? left, Uri? right) => AreEqual(left, right);
    
    /// <summary>
    ///     Compares an ASUri against a native Uri.
    /// </summary>
    public static bool operator !=(ASUri? left, Uri? right) => !AreEqual(left, right);

    /// <summary>
    ///     Compares an ASUri against a string.
    /// </summary>
    public static bool operator ==(ASUri? left, string? right) => AreEqual(left, right);
    
    /// <summary>
    ///     Compares an ASUri against a string.
    /// </summary>
    public static bool operator !=(ASUri? left, string? right) => !AreEqual(left, right);
    
    /// <summary>
    ///     Compares two ASUri objects for equality.
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
    ///     Compares an ASUri against a native Uri.
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
    ///     Compares an ASUri against a string.
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
    ///     Parses an ASUri from a string
    /// </summary>
    public static implicit operator string(ASUri asUri) => asUri.ToString();

    /// <summary>
    ///     Converts this ASUri to its string value
    /// </summary>
    public static implicit operator ASUri(string str) => new(str);

    /// <summary>
    ///     Converts a native Uri into a an ASUri
    /// </summary>
    public static implicit operator ASUri(Uri uri) => new(uri);
    
    /// <summary>
    ///     Converts an ASUri into a native Uri
    /// </summary>
    public static implicit operator Uri(ASUri asUri) => asUri.Uri;
}