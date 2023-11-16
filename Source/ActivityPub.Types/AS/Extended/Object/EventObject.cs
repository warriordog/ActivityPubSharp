// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System.Diagnostics.CodeAnalysis;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     Represents any kind of event.
/// </summary>
public class EventObject : ASObject, IASModel<EventObject, EventObjectEntity, ASObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Event" types.
    /// </summary>
    public const string EventType = "Event";
    static string IASModel<EventObject>.ASTypeName => EventType;

    /// <summary>
    ///     Constructs a new instance and attaches it to a new, empty type graph.
    /// </summary>
    public EventObject() : this(new TypeMap()) {}

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public EventObject(TypeMap typeMap) : base(typeMap)
        => Entity = TypeMap.Extend<EventObjectEntity>();

    /// <summary>
    ///     Constructs a new instance and extends an existing type graph from a provided model.
    /// </summary>
    /// <seealso cref="TypeMap.Extend{TEntity}()" />
    public EventObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <summary>
    ///     Constructs a new instance using entities from an existing type graph.
    /// </summary>
    [SetsRequiredMembers]
    public EventObject(TypeMap typeMap, EventObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<EventObjectEntity>();

    static EventObject IASModel<EventObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


    private EventObjectEntity Entity { get; }
}

/// <inheritdoc cref="EventObject" />
public sealed class EventObjectEntity : ASEntity<EventObject, EventObjectEntity> {}