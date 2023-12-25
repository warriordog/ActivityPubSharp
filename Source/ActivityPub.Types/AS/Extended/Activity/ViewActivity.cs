// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has viewed the object.
/// </summary>
public class ViewActivity : ASActivity, IASModel<ViewActivity, ViewActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "View" types.
    /// </summary>
    [PublicAPI]
    public const string ViewType = "View";
    static string IASModel<ViewActivity>.ASTypeName => ViewType;

    /// <inheritdoc />
    public ViewActivity() => Entity = TypeMap.Extend<ViewActivity, ViewActivityEntity>();

    /// <inheritdoc />
    public ViewActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<ViewActivity, ViewActivityEntity>(isExtending);

    /// <inheritdoc />
    public ViewActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public ViewActivity(TypeMap typeMap, ViewActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ViewActivity, ViewActivityEntity>();

    static ViewActivity IASModel<ViewActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ViewActivityEntity Entity { get; }
}

/// <inheritdoc cref="ViewActivity" />
public sealed class ViewActivityEntity : ASEntity<ViewActivity, ViewActivityEntity> {}