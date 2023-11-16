// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.AS.Extended.Link;

/// <summary>
///     A specialized Link that represents an @mention.
/// </summary>
public class MentionLink : ASLink, IASModel<MentionLink, MentionLinkEntity, ASLink>
{
    public const string MentionType = "Mention";
    static string IASModel<MentionLink>.ASTypeName => MentionType;

    public MentionLink() : this(new TypeMap()) {}

    public MentionLink(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<MentionLinkEntity>();

    public MentionLink(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public MentionLink(TypeMap typeMap, MentionLinkEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<MentionLinkEntity>();

    static MentionLink IASModel<MentionLink>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private MentionLinkEntity Entity { get; }


    public static implicit operator string(MentionLink link) => link.HRef;
    public static implicit operator MentionLink(string str) => new() { HRef = new ASUri(str) };

    public static implicit operator Uri(MentionLink link) => link.HRef.Uri;
    public static implicit operator MentionLink(Uri uri) => new() { HRef = new ASUri(uri) };

    public static implicit operator ASUri(MentionLink link) => link.HRef;
    public static implicit operator MentionLink(ASUri asUri) => new() { HRef = asUri };
}

/// <summary>
///     A specialized Link that represents an @mention.
/// </summary>
public sealed class MentionLinkEntity : ASEntity<MentionLink, MentionLinkEntity> {}