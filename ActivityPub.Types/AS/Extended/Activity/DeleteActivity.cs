// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has deleted the object.
///     If specified, the origin indicates the context from which the object was deleted.
/// </summary>
public class DeleteActivity : ASTransitiveActivity
{
    public DeleteActivity() => Entity = new DeleteActivityEntity { TypeMap = TypeMap };
    public DeleteActivity(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<DeleteActivityEntity>();
    private DeleteActivityEntity Entity { get; }
}

/// <inheritdoc cref="DeleteActivity" />
[APType(DeleteType)]
[ImpliesOtherEntity(typeof(ASTransitiveActivityEntity))]
public sealed class DeleteActivityEntity : ASEntity<DeleteActivity>
{
    public const string DeleteType = "Delete";
    public override string ASTypeName => DeleteType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASActivityEntity.ActivityType
    };
}