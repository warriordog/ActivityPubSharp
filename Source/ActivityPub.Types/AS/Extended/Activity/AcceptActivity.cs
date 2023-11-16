// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor accepts the object.
///     The target property can be used in certain circumstances to indicate the context into which the object has been accepted.
/// </summary>
public class AcceptActivity : ASTransitiveActivity, IASModel<AcceptActivity, AcceptActivityEntity, ASTransitiveActivity>
{
    public const string AcceptType = "Accept";
    static string IASModel<AcceptActivity>.ASTypeName => AcceptType;

    public AcceptActivity() : this(new TypeMap()) {}

    public AcceptActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new AcceptActivityEntity();
        TypeMap.Add(Entity);
    }

    public AcceptActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public AcceptActivity(TypeMap typeMap, AcceptActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<AcceptActivityEntity>();

    static AcceptActivity IASModel<AcceptActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private AcceptActivityEntity Entity { get; }
}

/// <inheritdoc cref="AcceptActivity" />
public sealed class AcceptActivityEntity : ASEntity<AcceptActivity, AcceptActivityEntity> {}