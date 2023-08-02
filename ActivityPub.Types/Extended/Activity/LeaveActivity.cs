// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

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
public sealed class LeaveActivityEntity : ASBase
{
    public const string LeaveType = "Leave";

        /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public LeaveActivityEntity(TypeMap typeMap) : base(LeaveType, typeMap) {}
    
    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public LeaveActivityEntity() : base(LeaveType) {}
}