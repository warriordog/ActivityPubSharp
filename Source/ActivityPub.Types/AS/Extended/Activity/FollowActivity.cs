// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is "following" the object.
///     Following is defined in the sense typically used within Social systems in which the actor is interested in any activity performed by or on the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class FollowActivity : ASTransitiveActivity, IASModel<FollowActivity, FollowActivityEntity, ASTransitiveActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Follow" types.
    /// </summary>
    public const string FollowType = "Follow";
    static string IASModel<FollowActivity>.ASTypeName => FollowType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public FollowActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public FollowActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<FollowActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public FollowActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public FollowActivity(TypeMap typeMap, FollowActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<FollowActivityEntity>();

    static FollowActivity IASModel<FollowActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private FollowActivityEntity Entity { get; }
}

/// <inheritdoc cref="FollowActivity" />
public sealed class FollowActivityEntity : ASEntity<FollowActivity, FollowActivityEntity> {}