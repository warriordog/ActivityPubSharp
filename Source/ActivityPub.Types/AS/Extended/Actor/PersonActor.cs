// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents an individual person.
/// </summary>
public class PersonActor : APActor, IASModel<PersonActor, PersonActorEntity, APActor>
{
    /// <summary>
    ///     ActivityStreams type name for "Person" types.
    /// </summary>
    [PublicAPI]
    public const string PersonType = "Person";
    static string IASModel<PersonActor>.ASTypeName => PersonType;

    /// <inheritdoc />
    public PersonActor() => Entity = TypeMap.Extend<PersonActor, PersonActorEntity>();

    /// <inheritdoc />
    public PersonActor(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<PersonActor, PersonActorEntity>(isExtending);

    /// <inheritdoc />
    public PersonActor(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public PersonActor(TypeMap typeMap, PersonActorEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<PersonActor, PersonActorEntity>();

    static PersonActor IASModel<PersonActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private PersonActorEntity Entity { get; }
}

/// <inheritdoc cref="PersonActor" />
public sealed class PersonActorEntity : ASEntity<PersonActor, PersonActorEntity> {}