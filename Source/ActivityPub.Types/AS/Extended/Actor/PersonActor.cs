// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents an individual person.
/// </summary>
public class PersonActor : APActor, IASModel<PersonActor, PersonActorEntity, APActor>
{
    /// <summary>
    ///     ActivityStreams type name for "Person" types.
    /// </summary>
    public const string PersonType = "Person";
    static string IASModel<PersonActor>.ASTypeName => PersonType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public PersonActor() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public PersonActor(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<PersonActorEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public PersonActor(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public PersonActor(TypeMap typeMap, PersonActorEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<PersonActorEntity>();

    static PersonActor IASModel<PersonActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private PersonActorEntity Entity { get; }
}

/// <inheritdoc cref="PersonActor" />
public sealed class PersonActorEntity : ASEntity<PersonActor, PersonActorEntity> {}