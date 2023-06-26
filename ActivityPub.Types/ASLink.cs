/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types;

/// <summary>
/// A Link is an indirect, qualified reference to a resource identified by a URL.
/// The fundamental model for links is established by <a href="https://tools.ietf.org/html/rfc5988">RFC5988</a>.
/// Many of the properties defined by the Activity Vocabulary allow values that are either instances of Object or Link.
/// When a Link is used, it establishes a qualified relation connecting the subject (the containing object) to the resource identified by the href.
/// Properties of the Link are properties of the reference as opposed to properties of the resource. 
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-link"/>
[JsonConverter(typeof(ASLinkConverter))]
[ASTypeKey(LinkType)]
public class ASLink : ASType
{
    public const string LinkType = "Link";

    [JsonConstructor]
    public ASLink() : this(LinkType) {}

    protected ASLink(string type) : base(type) {}

    /// <summary>
    /// The target resource pointed to by a Link. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-href"/>
    public required string HRef { get; set; }

    /// <summary>
    /// Hints as to the language used by the target resource.
    /// Value MUST be a [BCP47] Language-Tag. 
    /// </summary>
    /// <remarks>
    /// TODO make a dedicated type for Language-Tag. maybe use a library?
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-hreflang"/>
    public string? HRefLang { get; set; }

    /// <summary>
    /// On a Link, specifies a hint as to the rendering width in device-independent pixels of the linked resource. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-width"/>
    public int? Width { get; set; }

    /// <summary>
    /// On a Link, specifies a hint as to the rendering height in device-independent pixels of the linked resource. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-height"/>
    public int? Height { get; set; }

    /// <summary>
    /// A link relation associated with a Link.
    /// The value MUST conform to both the [HTML5] and [RFC5988] "link relation" definitions.
    /// In the [HTML5], any string not containing the "space" U+0020, "tab" (U+0009), "LF" (U+000A), "FF" (U+000C), "CR" (U+000D) or "," (U+002C) characters can be used as a valid link relation.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-rel"/>
    public List<string> Rel { get; set; } = new();

    public static implicit operator string(ASLink link) => link.HRef;
    public static implicit operator ASLink(string str) => new() { HRef = str };
}