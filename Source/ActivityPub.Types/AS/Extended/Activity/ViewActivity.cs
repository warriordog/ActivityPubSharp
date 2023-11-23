// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has viewed the object.
/// </summary>
public class ViewActivity : ASActivity, IASModel<ViewActivity, ViewActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "View" types.
    /// </summary>
    public const string ViewType = "View";
    static string IASModel<ViewActivity>.ASTypeName => ViewType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public ViewActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ViewActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<ViewActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ViewActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public ViewActivity(TypeMap typeMap, ViewActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ViewActivityEntity>();

    static ViewActivity IASModel<ViewActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ViewActivityEntity Entity { get; }
}

/// <inheritdoc cref="ViewActivity" />
public sealed class ViewActivityEntity : ASEntity<ViewActivity, ViewActivityEntity> {}