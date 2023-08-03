// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is offering the object.
/// If specified, the target indicates the entity to which the object is being offered. 
/// </summary>
[APType(OfferType)]
public class OfferActivity : ASTransitiveActivity
{
    public const string OfferType = "Offer";

    [JsonConstructor]
    public OfferActivity() : this(OfferType) {}

    protected OfferActivity(string type) : base(type) {}
}
