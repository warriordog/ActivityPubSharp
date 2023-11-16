// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has listened to the object.
/// </summary>
public class ListenActivity : ASTransitiveActivity, IASModel<ListenActivity, ListenActivityEntity, ASTransitiveActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Listen" types.
    /// </summary>
    public const string ListenType = "Listen";
    static string IASModel<ListenActivity>.ASTypeName => ListenType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public ListenActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ListenActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<ListenActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ListenActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public ListenActivity(TypeMap typeMap, ListenActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ListenActivityEntity>();

    static ListenActivity IASModel<ListenActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ListenActivityEntity Entity { get; }
}

/// <inheritdoc cref="ListenActivity" />
public sealed class ListenActivityEntity : ASEntity<ListenActivity, ListenActivityEntity> {}