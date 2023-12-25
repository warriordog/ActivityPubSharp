// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is removing the object.
///     If specified, the origin indicates the context from which the object is being removed.
/// </summary>
public class RemoveActivity : ASActivity, IASModel<RemoveActivity, RemoveActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Remove" types.
    /// </summary>
    [PublicAPI]
    public const string RemoveType = "Remove";
    static string IASModel<RemoveActivity>.ASTypeName => RemoveType;

    /// <inheritdoc />
    public RemoveActivity() => Entity = TypeMap.Extend<RemoveActivity, RemoveActivityEntity>();

    /// <inheritdoc />
    public RemoveActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<RemoveActivity, RemoveActivityEntity>(isExtending);

    /// <inheritdoc />
    public RemoveActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public RemoveActivity(TypeMap typeMap, RemoveActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<RemoveActivity, RemoveActivityEntity>();

    static RemoveActivity IASModel<RemoveActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private RemoveActivityEntity Entity { get; }
}

/// <inheritdoc cref="RemoveActivity" />
public sealed class RemoveActivityEntity : ASEntity<RemoveActivity, RemoveActivityEntity> {}