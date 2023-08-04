// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has joined the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class JoinActivity : ASTransitiveActivity
{
    public JoinActivity() => Entity = new JoinActivityEntity { TypeMap = TypeMap };
    public JoinActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<JoinActivityEntity>();
    private JoinActivityEntity Entity { get; }
}

/// <inheritdoc cref="JoinActivity" />
[ASTypeKey(JoinType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class JoinActivityEntity : ASBase<JoinActivity>
{
    public const string JoinType = "Join";
    public override string ASTypeName => JoinType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}