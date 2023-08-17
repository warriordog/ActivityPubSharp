// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     A specialization of Offer in which the actor is extending an invitation for the object to the target.
/// </summary>
public class InviteActivity : OfferActivity
{
    public InviteActivity() => Entity = new InviteActivityEntity { TypeMap = TypeMap };
    public InviteActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<InviteActivityEntity>();
    private InviteActivityEntity Entity { get; }
}

/// <inheritdoc cref="InviteActivity" />
[APType(InviteType)]
[ImpliesOtherEntity(typeof(OfferActivityEntity))]
public sealed class InviteActivityEntity : ASEntity<InviteActivity>
{
    public const string InviteType = "Invite";
    public override string ASTypeName => InviteType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        OfferActivityEntity.OfferType
    };
}