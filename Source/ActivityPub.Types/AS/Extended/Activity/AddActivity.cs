// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Indicates that the actor has added the object to the target.
///     If the target property is not explicitly specified, the target would need to be determined implicitly by context.
///     The origin can be used to identify the context from which the object originated.
/// </summary>
public class AddActivity : ASActivity, IASModel<AddActivity, AddActivityEntity, ASActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Add" types.
    /// </summary>
    [PublicAPI]
    public const string AddType = "Add";
    static string IASModel<AddActivity>.ASTypeName => AddType;

    /// <inheritdoc />
    public AddActivity() => Entity = TypeMap.Extend<AddActivity, AddActivityEntity>();

    /// <inheritdoc />
    public AddActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<AddActivity, AddActivityEntity>(isExtending);

    /// <inheritdoc />
    public AddActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public AddActivity(TypeMap typeMap, AddActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<AddActivity, AddActivityEntity>();

    static AddActivity IASModel<AddActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private AddActivityEntity Entity { get; }
}

/// <inheritdoc cref="AddActivity" />
public sealed class AddActivityEntity : ASEntity<AddActivity, AddActivityEntity> {}