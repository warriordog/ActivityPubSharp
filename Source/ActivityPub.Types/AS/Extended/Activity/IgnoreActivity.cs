// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is ignoring the object. The target and origin typically have no defined meaning.
/// </summary>
public class IgnoreActivity : ASActivity, IASModel<IgnoreActivity, IgnoreActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Ignore" types.
    /// </summary>
    public const string IgnoreType = "Ignore";
    static string IASModel<IgnoreActivity>.ASTypeName => IgnoreType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public IgnoreActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public IgnoreActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<IgnoreActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public IgnoreActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public IgnoreActivity(TypeMap typeMap, IgnoreActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<IgnoreActivityEntity>();

    static IgnoreActivity IASModel<IgnoreActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private IgnoreActivityEntity Entity { get; }
}

/// <inheritdoc cref="IgnoreActivity" />
public sealed class IgnoreActivityEntity : ASEntity<IgnoreActivity, IgnoreActivityEntity> {}