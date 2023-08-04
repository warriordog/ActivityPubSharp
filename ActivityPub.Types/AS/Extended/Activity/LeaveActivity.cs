// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has left the object.
///     The target and origin typically have no meaning.
/// </summary>
public class LeaveActivity : ASTransitiveActivity
{
    public LeaveActivity() => Entity = new LeaveActivityEntity { TypeMap = TypeMap };
    public LeaveActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<LeaveActivityEntity>();
    private LeaveActivityEntity Entity { get; }
}

/// <inheritdoc cref="LeaveActivity" />
[ASTypeKey(LeaveType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class LeaveActivityEntity : ASEntity<LeaveActivity>
{
    public const string LeaveType = "Leave";
    public override string ASTypeName => LeaveType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}