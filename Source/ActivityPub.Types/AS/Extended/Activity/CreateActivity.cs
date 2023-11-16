// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has created the object.
/// </summary>
public class CreateActivity : ASTransitiveActivity, IASModel<CreateActivity, CreateActivityEntity, ASTransitiveActivity>
{
    public const string CreateType = "Create";
    static string IASModel<CreateActivity>.ASTypeName => CreateType;

    public CreateActivity() : this(new TypeMap()) {}

    public CreateActivity(TypeMap typeMap) : base(typeMap)
    {
        Entity = new CreateActivityEntity();
        TypeMap.AddEntity(Entity);
    }

    public CreateActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public CreateActivity(TypeMap typeMap, CreateActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<CreateActivityEntity>();

    static CreateActivity IASModel<CreateActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private CreateActivityEntity Entity { get; }
}

/// <inheritdoc cref="CreateActivity" />
public sealed class CreateActivityEntity : ASEntity<CreateActivity, CreateActivityEntity> {}