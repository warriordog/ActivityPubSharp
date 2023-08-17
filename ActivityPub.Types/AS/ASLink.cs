// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.AS;

/// <summary>
///     A Link is an indirect, qualified reference to a resource identified by a URL.
///     The fundamental model for links is established by <a href="https://tools.ietf.org/html/rfc5988">RFC5988</a>.
///     Many of the properties defined by the Activity Vocabulary allow values that are either instances of Object or
///     When a Link is used, it establishes a qualified relation connecting the subject (the containing object) to the resource identified by the href.
///     Properties of the Link are properties of the reference as opposed to properties of the resource.
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-link" />
public class ASLink : ASType
{
    public ASLink() => Entity = new ASLinkEntity
    {
        TypeMap = TypeMap,
        HRef = null!
    };

    public ASLink(TypeMap typeMap) : base(typeMap) => Entity = typeMap.AsEntity<ASLinkEntity>();
    private ASLinkEntity Entity { get; }

    /// <summary>
    ///     The target resource pointed to by a
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-href" />
    public required ASUri HRef
    {
        get => Entity.HRef;
        set => Entity.HRef = value;
    }

    /// <summary>
    ///     Hints as to the language used by the target resource.
    ///     Value MUST be a [BCP47] Language-Tag.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-hreflang" />
    public string? HRefLang
    {
        get => Entity.HRefLang;
        set => Entity.HRefLang = value;
    }

    /// <summary>
    ///     On a Link, specifies a hint as to the rendering width in device-independent pixels of the linked resource.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-width" />
    public int? Width
    {
        get => Entity.Width;
        set => Entity.Width = value;
    }

    /// <summary>
    ///     On a Link, specifies a hint as to the rendering height in device-independent pixels of the linked resource.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-height" />
    public int? Height
    {
        get => Entity.Height;
        set => Entity.Height = value;
    }

    /// <summary>
    ///     A link relation associated with a
    ///     The value MUST conform to both the [HTML5] and [RFC5988] "link relation" definitions.
    ///     In the [HTML5], any string not containing the "space" U+0020, "tab" (U+0009), "LF" (U+000A), "FF" (U+000C), "CR" (U+000D) or "," (U+002C) characters can be used as a valid link relation.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-rel" />
    public HashSet<LinkRel> Rel
    {
        get => Entity.Rel;
        set => Entity.Rel = value;
    }

    public static implicit operator string(ASLink link) => link.HRef;
    public static implicit operator ASLink(string str) => new() { HRef = new ASUri(str) };

    public static implicit operator Uri(ASLink link) => link.HRef.Uri;
    public static implicit operator ASLink(Uri uri) => new() { HRef = new ASUri(uri) };

    public static implicit operator ASUri(ASLink link) => link.HRef;
    public static implicit operator ASLink(ASUri asUri) => new() { HRef = asUri };
}

/// <inheritdoc cref="ASLink" />
[APType(LinkType)]
[ImpliesOtherEntity(typeof(ASTypeEntity))]
public sealed class ASLinkEntity : ASEntity<ASLink>, ILinkEntity
{
    public const string LinkType = "Link";
    public override string ASTypeName => LinkType;

    /// <inheritdoc cref="ASLink.HRef" />
    [JsonPropertyName("href")]
    public required ASUri HRef { get; set; }

    /// <inheritdoc cref="ASLink.HRefLang" />
    [JsonPropertyName("hreflang")]
    public string? HRefLang { get; set; }

    /// <inheritdoc cref="ASLink.Width" />
    [JsonPropertyName("width")]
    public int? Width { get; set; }

    /// <inheritdoc cref="ASLink.Height" />
    [JsonPropertyName("height")]
    public int? Height { get; set; }

    /// <inheritdoc cref="ASLink.Rel" />
    [JsonPropertyName("rel")]
    public HashSet<LinkRel> Rel { get; set; } = new();

    public bool RequiresObjectForm => HRefLang != null || Width != null || Height != null || Rel.Count != 0;
}