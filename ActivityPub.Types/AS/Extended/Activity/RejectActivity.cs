// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is rejecting the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class RejectActivity : ASTransitiveActivity
{
    public RejectActivity() => Entity = new RejectActivityEntity { TypeMap = TypeMap };
    public RejectActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<RejectActivityEntity>();
    private RejectActivityEntity Entity { get; }
}

/// <inheritdoc cref="RejectActivity" />
[APType(RejectType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class RejectActivityEntity : ASEntity<RejectActivity>
{
    public const string RejectType = "Reject";
    public override string ASTypeName => RejectType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}