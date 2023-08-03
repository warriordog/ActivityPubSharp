// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has moved object from origin to target.
/// If the origin or target are not specified, either can be determined by context. 
/// </summary>
public class MoveActivity : ASTransitiveActivity
{
    private MoveActivityEntity Entity { get; }

    public MoveActivity() => Entity = new MoveActivityEntity(TypeMap);
    public MoveActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<MoveActivityEntity>();
}

/// <inheritdoc cref="MoveActivity"/>
[ASTypeKey(MoveType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class MoveActivityEntity : ASBase<MoveActivity>
{
    public const string MoveType = "Move";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASActivityEntity.ActivityType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public MoveActivityEntity(TypeMap typeMap) : base(typeMap, MoveType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public MoveActivityEntity() : base(MoveType, ReplacedTypes) {}
}