// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     A specialization of Reject in which the rejection is considered tentative.
/// </summary>
public class TentativeRejectActivity : RejectActivity
{
    public TentativeRejectActivity() => Entity = new TentativeRejectActivityEntity { TypeMap = TypeMap };
    public TentativeRejectActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<TentativeRejectActivityEntity>();
    private TentativeRejectActivityEntity Entity { get; }
}

/// <inheritdoc cref="TentativeRejectActivity" />
[ASTypeKey(TentativeRejectType)]
[ImpliesOtherEntity(typeof(RejectActivityEntity))]
public sealed class TentativeRejectActivityEntity : ASBase<TentativeRejectActivity>
{
    public const string TentativeRejectType = "TentativeReject";
    public override string ASTypeName => TentativeRejectType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        RejectActivityEntity.RejectType
    };
}