// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has joined the object.
///     The target and origin typically have no defined meaning.
/// </summary>
public class JoinActivity : ASActivity, IASModel<JoinActivity, JoinActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Join" types.
    /// </summary>
    [PublicAPI]
    public const string JoinType = "Join";
    static string IASModel<JoinActivity>.ASTypeName => JoinType;

    /// <inheritdoc />
    public JoinActivity() => Entity = TypeMap.Extend<JoinActivity, JoinActivityEntity>();

    /// <inheritdoc />
    public JoinActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<JoinActivity, JoinActivityEntity>(isExtending);

    /// <inheritdoc />
    public JoinActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public JoinActivity(TypeMap typeMap, JoinActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<JoinActivity, JoinActivityEntity>();

    static JoinActivity IASModel<JoinActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private JoinActivityEntity Entity { get; }
}

/// <inheritdoc cref="JoinActivity" />
public sealed class JoinActivityEntity : ASEntity<JoinActivity, JoinActivityEntity> {}