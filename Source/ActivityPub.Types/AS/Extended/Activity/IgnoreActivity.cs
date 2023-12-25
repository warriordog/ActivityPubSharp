// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor is ignoring the object. The target and origin typically have no defined meaning.
/// </summary>
public class IgnoreActivity : ASActivity, IASModel<IgnoreActivity, IgnoreActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Ignore" types.
    /// </summary>
    [PublicAPI]
    public const string IgnoreType = "Ignore";
    static string IASModel<IgnoreActivity>.ASTypeName => IgnoreType;

    /// <inheritdoc />
    public IgnoreActivity() => Entity = TypeMap.Extend<IgnoreActivity, IgnoreActivityEntity>();

    /// <inheritdoc />
    public IgnoreActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<IgnoreActivity, IgnoreActivityEntity>(isExtending);

    /// <inheritdoc />
    public IgnoreActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public IgnoreActivity(TypeMap typeMap, IgnoreActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<IgnoreActivity, IgnoreActivityEntity>();

    static IgnoreActivity IASModel<IgnoreActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private IgnoreActivityEntity Entity { get; }
}

/// <inheritdoc cref="IgnoreActivity" />
public sealed class IgnoreActivityEntity : ASEntity<IgnoreActivity, IgnoreActivityEntity> {}