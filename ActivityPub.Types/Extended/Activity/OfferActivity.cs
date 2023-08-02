// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is offering the object.
/// If specified, the target indicates the entity to which the object is being offered. 
/// </summary>
public class OfferActivity : ASTransitiveActivity
{
    private OfferActivityEntity Entity { get; }
    
    public OfferActivity() => Entity = new OfferActivityEntity(TypeMap);
    public OfferActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<OfferActivityEntity>();
}


/// <inheritdoc cref="OfferActivity"/>
[ASTypeKey(OfferType)]
public sealed class OfferActivityEntity : ASBase
{
    public const string OfferType = "Offer";

        /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public OfferActivityEntity(TypeMap typeMap) : base(OfferType, typeMap) {}
    
    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public OfferActivityEntity() : base(OfferType) {}
}