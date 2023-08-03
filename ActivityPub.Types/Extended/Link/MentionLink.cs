// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Extended.Link;

/// <summary>
/// A specialized Link that represents an @mention. 
/// </summary>
public class MentionLink : ASLink
{
    private MentionLinkEntity Entity { get; }


    public MentionLink() => Entity = new MentionLinkEntity(TypeMap);
    public MentionLink(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<MentionLinkEntity>();


    public static implicit operator string(MentionLink link) => link.HRef;
    public static implicit operator MentionLink(string str) => new() { HRef = new ASUri(str) };

    public static implicit operator Uri(MentionLink link) => link.HRef.Uri;
    public static implicit operator MentionLink(Uri uri) => new() { HRef = new ASUri(uri) };

    public static implicit operator ASUri(MentionLink link) => link.HRef;
    public static implicit operator MentionLink(ASUri asUri) => new() { HRef = asUri };
}

/// <summary>
/// A specialized Link that represents an @mention. 
/// </summary>
[ASTypeKey(MentionType)]
[ImpliesOtherEntity(typeof(ASLinkEntity))]
public sealed class MentionLinkEntity : ASBase<MentionLink>
{
    public const string MentionType = "Mention";

    /// <inheritdoc cref="ASBase{T}(string?, TypeMap)"/>
    public MentionLinkEntity(TypeMap typeMap) : base(MentionType, typeMap) {}

    /// <inheritdoc cref="ASBase{T}(string?)"/>
    [JsonConstructor]
    public MentionLinkEntity() : base(MentionType) {}
}