// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Describes a software application.
/// </summary>
public class ApplicationActor : APActor, IASModel<ApplicationActor, ApplicationActorEntity, APActor>
{
    /// <summary>
    ///     ActivityStreams type name for "Application" types.
    /// </summary>
    public const string ApplicationType = "Application";
    static string IASModel<ApplicationActor>.ASTypeName => ApplicationType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public ApplicationActor() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ApplicationActor(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<ApplicationActorEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public ApplicationActor(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public ApplicationActor(TypeMap typeMap, ApplicationActorEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ApplicationActorEntity>();

    static ApplicationActor IASModel<ApplicationActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private ApplicationActorEntity Entity { get; }
}

/// <inheritdoc cref="ApplicationActor" />
public sealed class ApplicationActorEntity : ASEntity<ApplicationActor, ApplicationActorEntity> {}