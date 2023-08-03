// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// An IntransitiveActivity that indicates that the actor has arrived at the location.
/// The origin can be used to identify the context from which the actor originated.
/// The target typically has no defined meaning. 
/// </summary>
public class ArriveActivity : ASIntransitiveActivity
{
    private ArriveActivityEntity Entity { get; }

    public ArriveActivity() => Entity = new ArriveActivityEntity(TypeMap);
    public ArriveActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ArriveActivityEntity>();
}

/// <inheritdoc cref="ArriveActivity"/>
[ASTypeKey(ArriveType)]
[ImpliesOtherEntity(typeof(ASIntransitiveActivityEntity))]
public sealed class ArriveActivityEntity : ASBase<ArriveActivity>
{
    public const string ArriveType = "Arrive";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASIntransitiveActivityEntity.IntransitiveActivityType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public ArriveActivityEntity(TypeMap typeMap) : base(typeMap, ArriveType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public ArriveActivityEntity() : base(ArriveType, ReplacedTypes) {}
}