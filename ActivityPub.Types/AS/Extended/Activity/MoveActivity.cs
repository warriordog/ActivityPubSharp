// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has moved object from origin to target.
///     If the origin or target are not specified, either can be determined by context.
/// </summary>
public class MoveActivity : ASTransitiveActivity
{
    public MoveActivity() => Entity = new MoveActivityEntity { TypeMap = TypeMap };
    public MoveActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<MoveActivityEntity>();
    private MoveActivityEntity Entity { get; }
}

/// <inheritdoc cref="MoveActivity" />
[ASTypeKey(MoveType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class MoveActivityEntity : ASBase<MoveActivity>
{
    public const string MoveType = "Move";
    public override string ASTypeName => MoveType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}