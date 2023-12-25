// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     A specialization of Offer in which the actor is extending an invitation for the object to the target.
/// </summary>
public class InviteActivity : OfferActivity, IASModel<InviteActivity, InviteActivityEntity, OfferActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Invite" types.
    /// </summary>
    [PublicAPI]
    public const string InviteType = "Invite";
    static string IASModel<InviteActivity>.ASTypeName => InviteType;

    /// <inheritdoc />
    public InviteActivity() => Entity = TypeMap.Extend<InviteActivity, InviteActivityEntity>();

    /// <inheritdoc />
    public InviteActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<InviteActivity, InviteActivityEntity>(isExtending);

    /// <inheritdoc />
    public InviteActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public InviteActivity(TypeMap typeMap, InviteActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<InviteActivity, InviteActivityEntity>();

    static InviteActivity IASModel<InviteActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private InviteActivityEntity Entity { get; }
}

/// <inheritdoc cref="InviteActivity" />
public sealed class InviteActivityEntity : ASEntity<InviteActivity, InviteActivityEntity> {}