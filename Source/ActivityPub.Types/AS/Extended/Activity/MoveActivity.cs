// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has moved object from origin to target.
///     If the origin or target are not specified, either can be determined by context.
/// </summary>
public class MoveActivity : ASTransitiveActivity, IASModel<MoveActivity, MoveActivityEntity, ASTransitiveActivity>
{
    public const string MoveType = "Move";
    static string IASModel<MoveActivity>.ASTypeName => MoveType;

    public MoveActivity() : this(new TypeMap()) {}

    public MoveActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new MoveActivityEntity();
        TypeMap.Add(Entity);
    }

    public MoveActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public MoveActivity(TypeMap typeMap, MoveActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<MoveActivityEntity>();

    static MoveActivity IASModel<MoveActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private MoveActivityEntity Entity { get; }
}

/// <inheritdoc cref="MoveActivity" />
public sealed class MoveActivityEntity : ASEntity<MoveActivity, MoveActivityEntity> {}