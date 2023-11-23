// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has joined the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class JoinActivity : ASActivity, IASModel<JoinActivity, JoinActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Join" types.
    /// </summary>
    public const string JoinType = "Join";
    static string IASModel<JoinActivity>.ASTypeName => JoinType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public JoinActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public JoinActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<JoinActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public JoinActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public JoinActivity(TypeMap typeMap, JoinActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<JoinActivityEntity>();

    static JoinActivity IASModel<JoinActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private JoinActivityEntity Entity { get; }
}

/// <inheritdoc cref="JoinActivity" />
public sealed class JoinActivityEntity : ASEntity<JoinActivity, JoinActivityEntity> {}