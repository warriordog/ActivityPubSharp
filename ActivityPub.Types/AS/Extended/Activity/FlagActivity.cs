// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is "flagging" the object.
///     Flagging is defined in the sense common to many social platforms as reporting content as being inappropriate for any number of reasons.
/// </summary>
public class FlagActivity : ASTransitiveActivity
{
    public FlagActivity() => Entity = new FlagActivityEntity { TypeMap = TypeMap };
    public FlagActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<FlagActivityEntity>();
    private FlagActivityEntity Entity { get; }
}

/// <inheritdoc cref="FlagActivity" />
[ASTypeKey(FlagType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class FlagActivityEntity : ASEntity<FlagActivity>
{
    public const string FlagType = "Flag";
    public override string ASTypeName => FlagType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}