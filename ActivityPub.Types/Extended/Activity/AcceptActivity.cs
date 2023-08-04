// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
///     Indicates that the actor accepts the object.
///     The target property can be used in certain circumstances to indicate the context into which the object has been accepted.
/// </summary>
public class AcceptActivity : ASTransitiveActivity
{
    public AcceptActivity() => Entity = new AcceptActivityEntity { TypeMap = TypeMap };
    public AcceptActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<AcceptActivityEntity>();
    private AcceptActivityEntity Entity { get; }
}

/// <inheritdoc cref="AcceptActivity" />
[ASTypeKey(AcceptType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class AcceptActivityEntity : ASBase<AcceptActivity>
{
    public const string AcceptType = "Accept";
    public override string ASTypeName => AcceptType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}