// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types;

/// <summary>
/// A Link is an indirect, qualified reference to a resource identified by a URL.
/// The fundamental model for links is established by <a href="https://tools.ietf.org/html/rfc5988">RFC5988</a>.
/// Many of the properties defined by the Activity Vocabulary allow values that are either instances of Object or 
/// When a Link is used, it establishes a qualified relation connecting the subject (the containing object) to the resource identified by the href.
/// Properties of the Link are properties of the reference as opposed to properties of the resource. 
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-link"/>
[ASTypeKey(LinkType)]
public class ASLink : ASType
{
    public const string LinkType = "Link";

    [JsonConstructor]
    public ASLink() : this(LinkType) {}

    protected ASLink(string type) : base(type) {}

    /// <summary>
    /// The target resource pointed to by a  
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-href"/>
    [JsonPropertyName("href")]
    public required ASUri HRef { get; set; }

    /// <summary>
    /// Hints as to the language used by the target resource.
    /// Value MUST be a [BCP47] Language-Tag. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-hreflang"/>
    [JsonPropertyName("hreflang")]
    public string? HRefLang { get; set; }

    /// <summary>
    /// On a Link, specifies a hint as to the rendering width in device-independent pixels of the linked resource. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-width"/>
    [JsonPropertyName("width")]
    public int? Width { get; set; }

    /// <summary>
    /// On a Link, specifies a hint as to the rendering height in device-independent pixels of the linked resource. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-height"/>
    [JsonPropertyName("height")]
    public int? Height { get; set; }

    /// <summary>
    /// A link relation associated with a 
    /// The value MUST conform to both the [HTML5] and [RFC5988] "link relation" definitions.
    /// In the [HTML5], any string not containing the "space" U+0020, "tab" (U+0009), "LF" (U+000A), "FF" (U+000C), "CR" (U+000D) or "," (U+002C) characters can be used as a valid link relation.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-rel"/>
    [JsonPropertyName("rel")]
    public HashSet<LinkRel> Rel { get; set; } = new();

    public static implicit operator string(ASLink link) => link.HRef;
    public static implicit operator ASLink(string str) => new() { HRef = new ASUri(str) };

    public static implicit operator Uri(ASLink link) => link.HRef.Uri;
    public static implicit operator ASLink(Uri uri) => new() { HRef = new ASUri(uri) };

    public static implicit operator ASUri(ASLink link) => link.HRef;
    public static implicit operator ASLink(ASUri asUri) => new() { HRef = asUri };

    [CustomJsonDeserializer]
    public static bool TryDeserialize(JsonElement element, JsonSerializerOptions options, out ASLink? obj)
    {
        // We either parse from string, or allow parser to use default logic
        if (element.ValueKind == JsonValueKind.String)
        {
            obj = new ASLink
            {
                HRef = element.GetString()!
            };
            return true;
        }


        obj = null;
        return false;
    }

    [CustomJsonSerializer]
    public static bool TrySerialize(ASLink obj, JsonSerializerOptions options, JsonNodeOptions nodeOptions, [NotNullWhen(true)] out JsonNode? node)
    {
        // If its only a link, then use the flattened form
        if (obj.HasOnlyHRef)
        {
            node = JsonValue.Create(obj.HRef.Uri.ToString(), nodeOptions)!;
            return true;
        }

        node = null;
        return false;
    }

    /// <summary>
    /// True if a link contains a value for <see cref="HRef"/> only and can therefore be reduced.
    /// </summary>
    /// <remarks>
    /// Its fragile and must be updated whenever <see cref="ASLink"/> or <see cref="ASType"/> is updated.
    /// </remarks> 
    private bool HasOnlyHRef =>
        HRefLang == null && Width == null && Height == null && Rel.Count == 0 && Id == null &&
        AttributedTo.Count == 0 && Preview == null && Name == null && MediaType == null;
}