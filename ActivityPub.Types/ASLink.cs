// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ActivityPub.Types.Internal;
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
[JsonConverter(typeof(ASLinkConverter))]
[ASTypeKey(LinkType)]
public class ASLink : ASType, IJsonConvertible<ASLink>
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
    /// <remarks>
    /// TODO make a dedicated type for Language-Tag. maybe use a library?
    /// </remarks>
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
    public HashSet<string> Rel { get; set; } = new();

    public static implicit operator string(ASLink link) => link.HRef;
    public static implicit operator ASLink(string str) => new() { HRef = new ASUri(str) };

    public static implicit operator Uri(ASLink link) => link.HRef.Uri;
    public static implicit operator ASLink(Uri uri) => new() { HRef = new ASUri(uri) };

    public static implicit operator ASUri(ASLink link) => link.HRef;
    public static implicit operator ASLink(ASUri asUri) => new() { HRef = asUri };


    protected override void ReadJson(JsonElement element, JsonOptions options)
    {
        base.ReadJson(element, options);

        // Skip HRef - it has to be pre-initialized
        if (element.TryGetProperty("hreflang", out var hRefLang))
            HRefLang = hRefLang.GetString();
        if (element.TryGetProperty("width", out var width))
            Width = width.GetInt32();
        if (element.TryGetProperty("height", out var height))
            Height = height.GetInt32();
        if (element.TryGetProperty("rel", out var rel))
            Rel = rel.Deserialize<HashSet<string>>(options.SerializerOptions)!;
    }

    protected override void WriteJson(JsonNode node, JsonOptions options)
    {
        base.WriteJson(node, options);

        node["href"] = JsonValue.Create(HRef, options.NodeOptions);
        if (HRefLang != null)
            node["hreflang"] = JsonValue.Create(HRefLang, options.NodeOptions);
        if (Width != null)
            node["width"] = JsonValue.Create(Width, options.NodeOptions);
        if (Height != null)
            node["height"] = JsonValue.Create(Height, options.NodeOptions);
        if (Rel.Count > 0)
            node["rel"] = JsonSerializer.SerializeToNode(Rel, options.SerializerOptions);
    }

    public new static ASLink? Deserialize(JsonElement element, JsonOptions options)
    {
        if (element.ValueKind == JsonValueKind.Null)
            return null;

        // Parse flattened form
        if (element.ValueKind == JsonValueKind.String)
        {
            return new ASLink()
            {
                HRef = element.GetString()!
            };
        }

        if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty("href", out var href) && href.TryGetString(out var hrefString))
        {
            var link = new ASLink
            {
                HRef = hrefString
            };
            link.ReadJson(element, options);
            return link;
        }

        throw new JsonException("Can't deserialize ASLink - not in any supported form");
    }

    public static JsonNode? Serialize(ASLink obj, JsonOptions options)
    {
        // TODO split ASLink into flat and object form

        // If its only a link, then use the flattened form
        if (obj.HasOnlyHRef)
        {
            return JsonValue.Create(obj.HRef.Uri.ToString(), options.NodeOptions);
        }

        var node = new JsonObject(options.NodeOptions);
        obj.WriteJson(node, options);
        return node;
    }

    /// <summary>
    /// True if a link contains a value for <see cref="HRef"/> only and can therefore be reduced.
    /// </summary>
    /// <remarks>
    /// TODO: This is really a hack and should be replaced.
    /// Its fragile and must be updated whenever <see cref="ASLink"/> or <see cref="ASType"/> is updated.
    /// </remarks> 
    private bool HasOnlyHRef =>
        HRefLang == null && Width == null && Height == null && Rel.Count == 0 && Id == null &&
        AttributedTo.Count == 0 && Preview == null && Name == null && MediaType == null;
}