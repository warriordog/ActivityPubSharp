// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is "flagging" the object.
///     Flagging is defined in the sense common to many social platforms as reporting content as being inappropriate for any number of reasons.
/// </summary>
public class FlagActivity : ASActivity, IASModel<FlagActivity, FlagActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Flag" types.
    /// </summary>
    public const string FlagType = "Flag";
    static string IASModel<FlagActivity>.ASTypeName => FlagType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public FlagActivity() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public FlagActivity(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<FlagActivityEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public FlagActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public FlagActivity(TypeMap typeMap, FlagActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<FlagActivityEntity>();

    static FlagActivity IASModel<FlagActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private FlagActivityEntity Entity { get; }
}

/// <inheritdoc cref="FlagActivity" />
public sealed class FlagActivityEntity : ASEntity<FlagActivity, FlagActivityEntity> {}