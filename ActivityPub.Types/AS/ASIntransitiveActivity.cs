// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS;

/// <summary>
///     Instances of IntransitiveActivity are a subtype of Activity representing intransitive actions.
///     The object property is therefore inappropriate for these activities.
/// </summary>
/// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-intransitiveactivity" />
public class ASIntransitiveActivity : ASActivity
{
    public ASIntransitiveActivity() => Entity = new ASIntransitiveActivityEntity { TypeMap = TypeMap };
    public ASIntransitiveActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ASIntransitiveActivityEntity>();
    private ASIntransitiveActivityEntity Entity { get; }
}

/// <inheritdoc cref="ASIntransitiveActivity" />
[APType(IntransitiveActivityType)]
[ImpliesOtherEntity(typeof(ASActivityEntity))]
public sealed class ASIntransitiveActivityEntity : ASEntity<ASIntransitiveActivity>
{
    public const string IntransitiveActivityType = "IntransitiveActivity";
    public override string ASTypeName => IntransitiveActivityType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}