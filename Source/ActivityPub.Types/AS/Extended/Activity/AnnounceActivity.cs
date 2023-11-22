// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is calling the target's attention the object.
///     The origin typically has no defined meaning.
/// </summary>
public class AnnounceActivity : ASActivity, IASModel<AnnounceActivity, AnnounceActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Announce" types.
    /// </summary>
    public const string AnnounceType = "Announce";
    static string IASModel<AnnounceActivity>.ASTypeName => AnnounceType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public AnnounceActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public AnnounceActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<AnnounceActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public AnnounceActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public AnnounceActivity(TypeMap typeMap, AnnounceActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<AnnounceActivityEntity>();

    static AnnounceActivity IASModel<AnnounceActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private AnnounceActivityEntity Entity { get; }
}

/// <inheritdoc cref="AnnounceActivity" />
public sealed class AnnounceActivityEntity : ASEntity<AnnounceActivity, AnnounceActivityEntity> {}