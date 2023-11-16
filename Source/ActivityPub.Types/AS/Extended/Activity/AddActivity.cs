// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has added the object to the target.
///     If the target property is not explicitly specified, the target would need to be determined implicitly by context.
///     The origin can be used to identify the context from which the object originated.
/// </summary>
public class AddActivity : ASTargetedActivity, IASModel<AddActivity, AddActivityEntity, ASTargetedActivity>
{
    public const string AddType = "Add";
    static string IASModel<AddActivity>.ASTypeName => AddType;

    public AddActivity() : this(new TypeMap()) {}

    public AddActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new AddActivityEntity();
        TypeMap.AddEntity(Entity);
    }

    public AddActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public AddActivity(TypeMap typeMap, AddActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<AddActivityEntity>();

    static AddActivity IASModel<AddActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private AddActivityEntity Entity { get; }
}

/// <inheritdoc cref="AddActivity" />
public sealed class AddActivityEntity : ASEntity<AddActivity, AddActivityEntity> {}