// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has read the object.
/// </summary>
public class ReadActivity : ASActivity, IASModel<ReadActivity, ReadActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Read" types.
    /// </summary>
    public const string ReadType = "Read";
    static string IASModel<ReadActivity>.ASTypeName => ReadType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public ReadActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ReadActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<ReadActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ReadActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public ReadActivity(TypeMap typeMap, ReadActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ReadActivityEntity>();

    static ReadActivity IASModel<ReadActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ReadActivityEntity Entity { get; }
}

/// <inheritdoc cref="ReadActivity" />
public sealed class ReadActivityEntity : ASEntity<ReadActivity, ReadActivityEntity> {}