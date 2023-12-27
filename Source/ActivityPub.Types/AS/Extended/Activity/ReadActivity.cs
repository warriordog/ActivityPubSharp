// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has read the object.
/// </summary>
public class ReadActivity : ASActivity, IASModel<ReadActivity, ReadActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Read" types.
    /// </summary>
    [PublicAPI]
    public const string ReadType = "Read";
    static string IASModel<ReadActivity>.ASTypeName => ReadType;

    /// <inheritdoc />
    public ReadActivity() => Entity = TypeMap.Extend<ReadActivity, ReadActivityEntity>();

    /// <inheritdoc />
    public ReadActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<ReadActivity, ReadActivityEntity>(isExtending);

    /// <inheritdoc />
    public ReadActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public ReadActivity(TypeMap typeMap, ReadActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ReadActivity, ReadActivityEntity>();

    static ReadActivity IASModel<ReadActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ReadActivityEntity Entity { get; }
}

/// <inheritdoc cref="ReadActivity" />
public sealed class ReadActivityEntity : ASEntity<ReadActivity, ReadActivityEntity> {}