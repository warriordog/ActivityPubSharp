using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Util;

/// <summary>
/// Synthetic type to represent a URI or IRI as used in ActivityStreams.
/// Currently, this only handles "true" non-compacted URIs.
/// </summary>
[JsonConverter(typeof(ASUriConverter))]
public class ASUri : IEquatable<ASUri>, IEquatable<Uri>, IEquatable<string>
{
    public Uri Uri { get; }

    public ASUri(Uri uri) => Uri = uri;
    public ASUri(string uri) => Uri = new(uri);

    public override string ToString() => Uri.ToString();
    public override int GetHashCode() => Uri.GetHashCode();
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        
        if (obj is ASUri asUri)
            return Equals(asUri);
        if (obj is Uri uri)
            return Equals(uri);
        if (obj is string str)
            return Equals(str);

        return false;
    }
    public bool Equals(ASUri? other) => AreEqual(this, other);
    public bool Equals(Uri? other) => AreEqual(this, other);
    public bool Equals(string? other) => AreEqual(this, other);

    public static bool operator ==(ASUri? left, ASUri? right) => AreEqual(left, right);
    public static bool operator !=(ASUri? left, ASUri? right) => !AreEqual(left, right);
    
    public static bool operator ==(ASUri? left, Uri? right) => AreEqual(left, right);
    public static bool operator !=(ASUri? left, Uri? right) => !AreEqual(left, right);
    
    public static bool operator ==(ASUri? left, string? right) => AreEqual(left, right);
    public static bool operator !=(ASUri? left, string? right) => !AreEqual(left, right);

    public static bool AreEqual(ASUri? left, ASUri? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (ReferenceEquals(null, right))
            return ReferenceEquals(null, left);
        if (ReferenceEquals(null, left))
            return ReferenceEquals(null, right);
        
        return left.Uri.Equals(right.Uri);
    }
    public static bool AreEqual(ASUri? left, Uri? right) {
        if (ReferenceEquals(null, right))
            return ReferenceEquals(null, left);
        if (ReferenceEquals(null, left))
            return ReferenceEquals(null, right);
        
        return left.Uri.Equals(right);
    }
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
    
    public static implicit operator string(ASUri asUri) => asUri.ToString();
    public static implicit operator ASUri(string str) => new(str);
    
    public static implicit operator ASUri(Uri uri) => new(uri);
    public static implicit operator Uri(ASUri asUri) => asUri.Uri;
}