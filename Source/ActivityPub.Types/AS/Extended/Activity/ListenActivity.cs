// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has listened to the object.
/// </summary>
public class ListenActivity : ASActivity, IASModel<ListenActivity, ListenActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Listen" types.
    /// </summary>
    [PublicAPI]
    public const string ListenType = "Listen";
    static string IASModel<ListenActivity>.ASTypeName => ListenType;

    /// <inheritdoc />
    public ListenActivity() => Entity = TypeMap.Extend<ListenActivity, ListenActivityEntity>();

    /// <inheritdoc />
    public ListenActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<ListenActivity, ListenActivityEntity>(isExtending);

    /// <inheritdoc />
    public ListenActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public ListenActivity(TypeMap typeMap, ListenActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ListenActivity, ListenActivityEntity>();

    static ListenActivity IASModel<ListenActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ListenActivityEntity Entity { get; }
}

/// <inheritdoc cref="ListenActivity" />
public sealed class ListenActivityEntity : ASEntity<ListenActivity, ListenActivityEntity> {}