// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Actor;

/// <summary>
///     Represents an individual person.
/// </summary>
public class PersonActor : APActor, IASModel<PersonActor, PersonActorEntity, APActor>
{
    public const string PersonType = "Person";
    static string IASModel<PersonActor>.ASTypeName => PersonType;

    public PersonActor() : this(new TypeMap()) {}

    public PersonActor(TypeMap typeMap) : base(typeMap)
    {
        Entity = new PersonActorEntity();
        TypeMap.Add(Entity);
    }

    public PersonActor(ASType existingGraph) : this(existingGraph.TypeMap) {}

    [SetsRequiredMembers]
    public PersonActor(TypeMap typeMap, PersonActorEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<PersonActorEntity>();

    static PersonActor IASModel<PersonActor>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private PersonActorEntity Entity { get; }
}

/// <inheritdoc cref="PersonActor" />
public sealed class PersonActorEntity : ASEntity<PersonActor, PersonActorEntity> {}