// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor has left the object.
/// The target and origin typically have no meaning.
/// </summary>
public class LeaveActivity : ASTransitiveActivity
{
    private LeaveActivityEntity Entity { get; }

    public LeaveActivity() => Entity = new LeaveActivityEntity(TypeMap);
    public LeaveActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<LeaveActivityEntity>();
}

/// <inheritdoc cref="LeaveActivity"/>
[ASTypeKey(LeaveType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class LeaveActivityEntity : ASBase<LeaveActivity>
{
    public const string LeaveType = "Leave";
    private static readonly IReadOnlySet<string> ReplacedTypes = new HashSet<string>()
    {
        ASActivityEntity.ActivityType
    };

    /// <inheritdoc cref="ASBase{TType}(ActivityPub.Types.TypeMap,string,System.Collections.Generic.IReadOnlySet{string}?)"/>
    public LeaveActivityEntity(TypeMap typeMap) : base(typeMap, LeaveType, ReplacedTypes) {}

    /// <inheritdoc cref="ASBase{T}(string, IReadOnlySet{string}?)"/>
    [JsonConstructor]
    public LeaveActivityEntity() : base(LeaveType, ReplacedTypes) {}
}