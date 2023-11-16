// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is removing the object.
///     If specified, the origin indicates the context from which the object is being removed.
/// </summary>
public class RemoveActivity : ASTargetedActivity, IASModel<RemoveActivity, RemoveActivityEntity, ASTargetedActivity>
{
    public const string RemoveType = "Remove";
    static string IASModel<RemoveActivity>.ASTypeName => RemoveType;

    public RemoveActivity() : this(new TypeMap()) {}

    public RemoveActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<RemoveActivityEntity>();

    public RemoveActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public RemoveActivity(TypeMap typeMap, RemoveActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<RemoveActivityEntity>();

    static RemoveActivity IASModel<RemoveActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private RemoveActivityEntity Entity { get; }
}

/// <inheritdoc cref="RemoveActivity" />
public sealed class RemoveActivityEntity : ASEntity<RemoveActivity, RemoveActivityEntity> {}