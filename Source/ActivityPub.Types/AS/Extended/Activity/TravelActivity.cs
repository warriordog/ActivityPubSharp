// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

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
    [PublicAPI]
    public const string TravelType = "Travel";
    static string IASModel<TravelActivity>.ASTypeName => TravelType;

    /// <inheritdoc />
    public TravelActivity() => Entity = TypeMap.Extend<TravelActivity, TravelActivityEntity>();

    /// <inheritdoc />
    public TravelActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<TravelActivity, TravelActivityEntity>(isExtending);

    /// <inheritdoc />
    public TravelActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public TravelActivity(TypeMap typeMap, TravelActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<TravelActivity, TravelActivityEntity>();

    static TravelActivity IASModel<TravelActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private TravelActivityEntity Entity { get; }
}

/// <inheritdoc cref="TravelActivity" />
public sealed class TravelActivityEntity : ASEntity<TravelActivity, TravelActivityEntity> {}