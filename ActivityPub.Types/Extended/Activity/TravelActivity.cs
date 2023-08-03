// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is traveling to target from origin.
/// Travel is an IntransitiveObject whose actor specifies the direct object.
/// If the target or origin are not specified, either can be determined by context.
/// </summary>
public class TravelActivity : ASIntransitiveActivity
{
    private TravelActivityEntity Entity { get; }

    public TravelActivity() => Entity = new TravelActivityEntity(TypeMap);
    public TravelActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<TravelActivityEntity>();
}

/// <inheritdoc cref="TravelActivity"/>
[ASTypeKey(TravelType)]
[ImpliesOtherEntity(typeof(ASIntransitiveActivityEntity))]
public sealed class TravelActivityEntity : ASBase<TravelActivity>
{
    public const string TravelType = "Travel";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASIntransitiveActivityEntity.IntransitiveActivityType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public TravelActivityEntity(TypeMap typeMap) : base(typeMap, TravelType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public TravelActivityEntity() : base(TravelType, ReplacedTypes) {}
}