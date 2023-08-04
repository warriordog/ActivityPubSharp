// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has viewed the object.
/// </summary>
public class ViewActivity : ASTransitiveActivity
{
    public ViewActivity() => Entity = new ViewActivityEntity { TypeMap = TypeMap };
    public ViewActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ViewActivityEntity>();
    private ViewActivityEntity Entity { get; }
}

/// <inheritdoc cref="ViewActivity" />
[ASTypeKey(ViewType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class ViewActivityEntity : ASBase<ViewActivity>
{
    public const string ViewType = "View";
    public override string ASTypeName => ViewType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}