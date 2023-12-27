// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Describes a software application.
/// </summary>
public class ApplicationActor : APActor, IASModel<ApplicationActor, ApplicationActorEntity, APActor>
{
    /// <summary>
    ///     ActivityStreams type name for "Application" types.
    /// </summary>
    [PublicAPI]
    public const string ApplicationType = "Application";
    static string IASModel<ApplicationActor>.ASTypeName => ApplicationType;

    /// <inheritdoc />
    public ApplicationActor() => Entity = TypeMap.Extend<ApplicationActor, ApplicationActorEntity>();

    /// <inheritdoc />
    public ApplicationActor(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<ApplicationActor, ApplicationActorEntity>(isExtending);

    /// <inheritdoc />
    public ApplicationActor(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public ApplicationActor(TypeMap typeMap, ApplicationActorEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ApplicationActor, ApplicationActorEntity>();

    static ApplicationActor IASModel<ApplicationActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ApplicationActorEntity Entity { get; }
}

/// <inheritdoc cref="ApplicationActor" />
public sealed class ApplicationActorEntity : ASEntity<ApplicationActor, ApplicationActorEntity> {}