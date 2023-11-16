// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has created the object.
/// </summary>
public class CreateActivity : ASTransitiveActivity, IASModel<CreateActivity, CreateActivityEntity, ASTransitiveActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Create" types.
    /// </summary>
    public const string CreateType = "Create";
    static string IASModel<CreateActivity>.ASTypeName => CreateType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public CreateActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public CreateActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<CreateActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public CreateActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public CreateActivity(TypeMap typeMap, CreateActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<CreateActivityEntity>();

    static CreateActivity IASModel<CreateActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private CreateActivityEntity Entity { get; }
}

/// <inheritdoc cref="CreateActivity" />
public sealed class CreateActivityEntity : ASEntity<CreateActivity, CreateActivityEntity> {}