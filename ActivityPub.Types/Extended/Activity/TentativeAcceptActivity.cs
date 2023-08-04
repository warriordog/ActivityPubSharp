// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// A specialization of TentativeAccept indicating that the tentativeAcceptance is tentative.
/// </summary>
public class TentativeAcceptActivity : AcceptActivity
{
    private TentativeAcceptActivityEntity Entity { get; }

    public TentativeAcceptActivity() => Entity = new TentativeAcceptActivityEntity { TypeMap = TypeMap };
    public TentativeAcceptActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<TentativeAcceptActivityEntity>();
}

/// <inheritdoc cref="TentativeAcceptActivity"/>
[ASTypeKey(TentativeAcceptType)]
[ImpliesOtherEntity(typeof(AcceptActivityEntity))]
public sealed class TentativeAcceptActivityEntity : ASBase<TentativeAcceptActivity>
{
    public const string TentativeAcceptType = "TentativeAccept";
    public override string ASTypeName => TentativeAcceptType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>()
    {
        AcceptActivityEntity.AcceptType
    };
}