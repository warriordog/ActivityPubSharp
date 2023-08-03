// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// A specialization of Offer in which the actor is extending an invitation for the object to the target. 
/// </summary>
public class InviteActivity : OfferActivity
{
    private InviteActivityEntity Entity { get; }

    public InviteActivity() => Entity = new InviteActivityEntity(TypeMap);
    public InviteActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<InviteActivityEntity>();
}

/// <inheritdoc cref="InviteActivity"/>
[ASTypeKey(InviteType)]
[ImpliesOtherEntity(typeof(OfferActivityEntity))]
public sealed class InviteActivityEntity : ASBase<InviteActivity>
{
    public const string InviteType = "Invite";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        OfferActivityEntity.OfferType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string?,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public InviteActivityEntity(TypeMap typeMap) : base(typeMap, InviteType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string?, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public InviteActivityEntity() : base(InviteType, ReplacedTypes) {}
}