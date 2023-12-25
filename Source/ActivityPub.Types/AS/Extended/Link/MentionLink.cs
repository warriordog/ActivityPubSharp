// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using ActivityPub.Types.Util;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Link;

/// <summary>
///     A specialized Link that represents an @mention.
/// </summary>
public class MentionLink : ASLink, IASModel<MentionLink, MentionLinkEntity, ASLink>
{
    /// <summary>
    ///     ActivityStreams type name for "Mention" types.
    /// </summary>
    [PublicAPI]
    public const string MentionType = "Mention";
    static string IASModel<MentionLink>.ASTypeName => MentionType;

    /// <inheritdoc />
    public MentionLink() => Entity = TypeMap.Extend<MentionLink, MentionLinkEntity>();

    /// <inheritdoc />
    public MentionLink(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<MentionLink, MentionLinkEntity>(isExtending);

    /// <inheritdoc />
    public MentionLink(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public MentionLink(TypeMap typeMap, MentionLinkEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<MentionLink, MentionLinkEntity>();

    static MentionLink IASModel<MentionLink>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private MentionLinkEntity Entity { get; }

    /// <summary>
    ///     Implicitly converts the link to a string using the value of <see cref="ASLink.HRef"/>.
    /// </summary>
    public static implicit operator string(MentionLink link) => link.HRef;

    /// <summary>
    ///     Implicitly converts a string into a link.
    ///     String value will be assigned to <see cref="ASLink.HRef"/>.
    /// </summary>
    public static implicit operator MentionLink(string str) => new() { HRef = new ASUri(str) };
    
    /// <summary>
    ///     Implicitly converts the link to a Uri using the value of <see cref="ASLink.HRef"/>.
    /// </summary>
    public static implicit operator Uri(MentionLink link) => link.HRef.Uri;
    
    /// <summary>
    ///     Implicitly converts a Uri into a link.
    ///     Uri value will be assigned to <see cref="ASLink.HRef"/>.
    /// </summary>
    public static implicit operator MentionLink(Uri uri) => new() { HRef = new ASUri(uri) };
    
    /// <summary>
    ///     Implicitly converts the link to an ASUri using the value of <see cref="ASLink.HRef"/>.
    /// </summary>
    public static implicit operator ASUri(MentionLink link) => link.HRef;
    
    /// <summary>
    ///     Implicitly converts an ASUri into a link.
    ///     Uri value will be assigned to <see cref="ASLink.HRef"/>.
    /// </summary>
    public static implicit operator MentionLink(ASUri asUri) => new() { HRef = asUri };
}

/// <summary>
///     A specialized Link that represents an @mention.
/// </summary>
public sealed class MentionLinkEntity : ASEntity<MentionLink, MentionLinkEntity> {}