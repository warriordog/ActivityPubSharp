/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// A specialization of Offer in which the actor is extending an invitation for the object to the target. 
/// </summary>
[ASTypeKey(InviteType)]
public class InviteActivity : OfferActivity
{
    public const string InviteType = "Invite";

    [JsonConstructor]
    public InviteActivity() : this(InviteType) {}

    protected InviteActivity(string type) : base(type) {}
}