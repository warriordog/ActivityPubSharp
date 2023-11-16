// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is traveling to target from origin.
///     Travel is an IntransitiveObject whose actor specifies the direct object.
///     If the target or origin are not specified, either can be determined by context.
/// </summary>
public class TravelActivity : ASIntransitiveActivity, IASModel<TravelActivity, TravelActivityEntity, ASIntransitiveActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Travel" types.
    /// </summary>
    public const string TravelType = "Travel";
    static string IASModel<TravelActivity>.ASTypeName => TravelType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public TravelActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public TravelActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<TravelActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public TravelActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public TravelActivity(TypeMap typeMap, TravelActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<TravelActivityEntity>();

    static TravelActivity IASModel<TravelActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private TravelActivityEntity Entity { get; }
}

/// <inheritdoc cref="TravelActivity" />
public sealed class TravelActivityEntity : ASEntity<TravelActivity, TravelActivityEntity> {}