// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Indicates that the actor is ignoring the object. The target and origin typically have no defined meaning. 
/// </summary>
public class IgnoreActivity : ASTransitiveActivity
{
    private IgnoreActivityEntity Entity { get; }

    public IgnoreActivity() => Entity = new IgnoreActivityEntity { TypeMap = TypeMap };
    public IgnoreActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<IgnoreActivityEntity>();
}

/// <inheritdoc cref="IgnoreActivity"/>
[ASTypeKey(IgnoreType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class IgnoreActivityEntity : ASBase<IgnoreActivity>
{
    public const string IgnoreType = "Ignore";
    public override string ASTypeName => IgnoreType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>()
    {
        ASActivityEntity.ActivityType
    };
}