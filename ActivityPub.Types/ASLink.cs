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
public class ASLink : ASType
{
    private ASLinkEntity Entity { get; }

    public ASLink() => Entity = new ASLinkEntity(TypeMap)
    {
        // Initialized by required property below
        // TODO see if there's a better way to do this
        HRef = null!
    };
    public ASLink(TypeMap typeMap) : base(typeMap) => Entity = typeMap.AsEntity<ASLinkEntity>();

    /// <summary>
    /// The target resource pointed to by a  
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-href"/>
    public required ASUri HRef
    {
        get => Entity.HRef;
        set => Entity.HRef = value;
    }

    /// <summary>
    /// Hints as to the language used by the target resource.
    /// Value MUST be a [BCP47] Language-Tag. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-hreflang"/>
    public string? HRefLang
    {
        get => Entity.HRefLang;
        set => Entity.HRefLang = value;
    }

    /// <summary>
    /// On a Link, specifies a hint as to the rendering width in device-independent pixels of the linked resource. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-width"/>
    public int? Width
    {
        get => Entity.Width;
        set => Entity.Width = value;
    }

    /// <summary>
    /// On a Link, specifies a hint as to the rendering height in device-independent pixels of the linked resource. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-height"/>
    public int? Height
    {
        get => Entity.Height;
        set => Entity.Height = value;
    }

    /// <summary>
    /// A link relation associated with a 
    /// The value MUST conform to both the [HTML5] and [RFC5988] "link relation" definitions.
    /// In the [HTML5], any string not containing the "space" U+0020, "tab" (U+0009), "LF" (U+000A), "FF" (U+000C), "CR" (U+000D) or "," (U+002C) characters can be used as a valid link relation.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-rel"/>
    public HashSet<LinkRel> Rel 
    {
        get => Entity.Rel;
        set => Entity.Rel = value;
    }

    // TODO these might wont work right, needs testing. Sub objects *should* be a different graph.
    public static implicit operator string(ASLink link) => link.HRef;
    public static implicit operator ASLink(string str) => new() { HRef = new ASUri(str) };

    public static implicit operator Uri(ASLink link) => link.HRef.Uri;
    public static implicit operator ASLink(Uri uri) => new() { HRef = new ASUri(uri) };

    public static implicit operator ASUri(ASLink link) => link.HRef;
    public static implicit operator ASLink(ASUri asUri) => new() { HRef = asUri };
}


/// <inheritdoc cref="ASLink"/>
[ASTypeKey(LinkType)]
[CustomJsonDeserializer(nameof(TryDeserialize))]
[CustomJsonSerializer(nameof(TrySerialize))]
public sealed class ASLinkEntity : ASBase
{
    public const string LinkType = "Link";
    
    private readonly ASTypeEntity _typeEntity;


    public ASLinkEntity(TypeMap typeMap) : base(LinkType, typeMap)
        => _typeEntity = typeMap.AsEntity<ASTypeEntity>();


    /// <inheritdoc cref="ASLink.HRef"/>
    [JsonPropertyName("href")]
    public required ASUri HRef { get; set; }

    /// <inheritdoc cref="ASLink.HRefLang"/>
    [JsonPropertyName("hreflang")]
    public string? HRefLang { get; set; }

    /// <inheritdoc cref="ASLink.Width"/>
    [JsonPropertyName("width")]
    public int? Width { get; set; }

    /// <inheritdoc cref="ASLink.Height"/>
    [JsonPropertyName("height")]
    public int? Height { get; set; }

    /// <inheritdoc cref="ASLink.Rel"/>
    [JsonPropertyName("rel")]
    public HashSet<LinkRel> Rel { get; set; } = new();

    public static bool TryDeserialize(JsonElement element, JsonSerializerOptions options, out ASLinkEntity? obj)
    {
        // We either parse from string, or allow parser to use default logic
        if (element.ValueKind == JsonValueKind.String)
        {
            obj = new ASLinkEntity
            {
                HRef = element.GetString()!
            };
            return true;
        }


        obj = null;
        return false;
    }

    public static bool TrySerialize(ASLinkEntity obj, JsonSerializerOptions options, JsonNodeOptions nodeOptions, [NotNullWhen(true)] out JsonNode? node)
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
        HRefLang == null && Width == null && Height == null && Rel.Count == 0 && _typeEntity.Id == null &&
        _typeEntity.AttributedTo.Count == 0 && _typeEntity.Preview == null && _typeEntity.Name == null && _typeEntity.MediaType == null && _typeEntity.UnknownJsonProperties.Count == 0;
}