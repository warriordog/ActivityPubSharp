// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has updated the object.
///     Note, however, that this vocabulary does not define a mechanism for describing the actual set of modifications made to object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class UpdateActivity : ASActivity, IASModel<UpdateActivity, UpdateActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Update" types.
    /// </summary>
    public const string UpdateType = "Update";
    static string IASModel<UpdateActivity>.ASTypeName => UpdateType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public UpdateActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public UpdateActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<UpdateActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public UpdateActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public UpdateActivity(TypeMap typeMap, UpdateActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<UpdateActivityEntity>();

    static UpdateActivity IASModel<UpdateActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private UpdateActivityEntity Entity { get; }
}

/// <inheritdoc cref="UpdateActivity" />
public sealed class UpdateActivityEntity : ASEntity<UpdateActivity, UpdateActivityEntity> {}