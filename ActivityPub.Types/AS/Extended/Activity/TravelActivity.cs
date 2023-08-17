// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is traveling to target from origin.
///     Travel is an IntransitiveObject whose actor specifies the direct object.
///     If the target or origin are not specified, either can be determined by context.
/// </summary>
public class TravelActivity : ASIntransitiveActivity
{
    public TravelActivity() => Entity = new TravelActivityEntity { TypeMap = TypeMap };
    public TravelActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<TravelActivityEntity>();
    private TravelActivityEntity Entity { get; }
}

/// <inheritdoc cref="TravelActivity" />
[APType(TravelType)]
[ImpliesOtherEntity(typeof(ASIntransitiveActivityEntity))]
public sealed class TravelActivityEntity : ASEntity<TravelActivity>
{
    public const string TravelType = "Travel";
    public override string ASTypeName => TravelType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASIntransitiveActivityEntity.IntransitiveActivityType
    };
}