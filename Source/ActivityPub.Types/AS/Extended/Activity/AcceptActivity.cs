// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor accepts the object.
///     The target property can be used in certain circumstances to indicate the context into which the object has been accepted.
/// </summary>
public class AcceptActivity : ASActivity, IASModel<AcceptActivity, AcceptActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Accept" types.
    /// </summary>
    [PublicAPI]
    public const string AcceptType = "Accept";
    static string IASModel<AcceptActivity>.ASTypeName => AcceptType;

    /// <inheritdoc />
    public AcceptActivity() => Entity = TypeMap.Extend<AcceptActivity, AcceptActivityEntity>();

    /// <inheritdoc />
    public AcceptActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<AcceptActivity, AcceptActivityEntity>(isExtending);

    /// <inheritdoc />
    public AcceptActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public AcceptActivity(TypeMap typeMap, AcceptActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<AcceptActivity, AcceptActivityEntity>();

    static AcceptActivity IASModel<AcceptActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private AcceptActivityEntity Entity { get; }
}

/// <inheritdoc cref="AcceptActivity" />
public sealed class AcceptActivityEntity : ASEntity<AcceptActivity, AcceptActivityEntity> {}