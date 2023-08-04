// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
///     Indicates that the actor has read the object.
/// </summary>
public class ReadActivity : ASTransitiveActivity
{
    public ReadActivity() => Entity = new ReadActivityEntity { TypeMap = TypeMap };
    public ReadActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ReadActivityEntity>();
    private ReadActivityEntity Entity { get; }
}

/// <inheritdoc cref="ReadActivity" />
[ASTypeKey(ReadType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class ReadActivityEntity : ASBase<ReadActivity>
{
    public const string ReadType = "Read";
    public override string ASTypeName => ReadType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}