// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

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
    [PublicAPI]
    public const string AnnounceType = "Announce";
    static string IASModel<AnnounceActivity>.ASTypeName => AnnounceType;

    /// <inheritdoc />
    public AnnounceActivity() => Entity = TypeMap.Extend<AnnounceActivity, AnnounceActivityEntity>();

    /// <inheritdoc />
    public AnnounceActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<AnnounceActivity, AnnounceActivityEntity>(isExtending);

    /// <inheritdoc />
    public AnnounceActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public AnnounceActivity(TypeMap typeMap, AnnounceActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<AnnounceActivity, AnnounceActivityEntity>();

    static AnnounceActivity IASModel<AnnounceActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private AnnounceActivityEntity Entity { get; }
}

/// <inheritdoc cref="AnnounceActivity" />
public sealed class AnnounceActivityEntity : ASEntity<AnnounceActivity, AnnounceActivityEntity> {}